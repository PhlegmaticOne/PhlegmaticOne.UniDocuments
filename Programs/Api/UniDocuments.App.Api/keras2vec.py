import os.path
import time

from keras.models import Model, load_model
from keras.layers import Layer, Embedding, Input, Concatenate, LSTM, Dense, Lambda, Average, Flatten, GRU, Dropout
from keras.callbacks import EarlyStopping, ModelCheckpoint
from keras.optimizers import Adam
from keras.utils import to_categorical
import numpy as np
import tensorflow as tf
import keras
import nltk
from nltk.corpus import stopwords as sw
import re
from typing import List, Dict
from sklearn.metrics.pairwise import cosine_similarity
import pickle
from pymorphy2 import MorphAnalyzer
import matplotlib.pyplot as plt

InputWordsLayerName = 'input_words'
InputDocumentsLayerName = 'input_documents'
InferredDocumentsLayerName = 'inferred_documents'
WordEmbeddingsLayerName = 'word_embeddings'
DocumentEmbeddingsLayerName = 'document_embeddings'
MergedLayerName = 'merged'
OutputLayerName = 'output'

nltk.download('stopwords')
stopwords = sw.words('russian')
analyzer = MorphAnalyzer()


def preprocess_and_tokenize(text: str, patterns: str) -> List[str]:
    doc = re.sub(patterns, ' ', text).lower()
    tokens: List[str] = []
    
    for token in doc.split():
        if token and token not in stopwords:
            token = token.strip()
            token = analyzer.normal_forms(token)[0]
            if len(token) > 1:
                tokens.append(token)
                
    return tokens


def get_path(input_data):
    path = input_data.Path
    name = input_data.Name
    result_name = "{0}.keras".format(name)
    return os.path.join(path, result_name)


def build_vocab(input_data):
    base_path = input_data.BasePath
    options = input_data.Options
    source = input_data.Source
    stream = DocumentsStreamSource(source, options)
    vocab = DocumentVocab.build_from_stream(stream)
    vocab.save(base_path)
    return vocab


def load_vocab(input_data):
    vocab = DocumentVocab.load(input_data)
    return vocab


def train(input_data):
    options = input_data.Options
    source = input_data.Source
    vocab = input_data.Vocab
    stream = DocumentsStreamSource(source, options)
    model = KerasDoc2VecModel(vocab, options, stream)
    model.build(is_infer=False)
    model.train(options.Epochs, options.LearningRate, options.Verbose)
    return model


def save(input_data):
    model = input_data.Model
    result_path = get_path(input_data)
    model.save(result_path)


def load(input_data):
    options = input_data.Options
    vocab = input_data.Vocab
    path = get_path(input_data)
    model = load_model(path, safe_mode=False)
    result = KerasDoc2VecModel(vocab, options, model=model)
    return result


def infer(input_data):
    model = input_data.Model
    text = input_data.Content
    options = input_data.Options
    top_n = input_data.TopN

    preprocess = preprocess_and_tokenize(text, patterns=options.TokenizeRegex)
    document = DocumentModel(0, preprocess)
    return model.infer_vector(document, top_n,
                              learning_rate=options.LearningRate,
                              epochs=input_data.InferEpochs,
                              verbose=options.Verbose)


class InferenceData(object):
    def __init__(self, cos, index):
        self.cos = cos
        self.index = index


class DocumentModel(object):

    def __init__(self, doc_id: int, tokens: List[str]):
        self.tokens = tokens
        self.doc_id = doc_id

    def generate_windows(self, window_size: int) -> Dict[str, List[str]]:
        windows: Dict[str, List[str]] = {}
        tokens = self.tokens
        window_offset = (window_size - 1) // 2
        words_count = len(tokens)

        for ix in range(window_offset, words_count - window_offset):
            curr_window = tokens[ix - window_offset:ix] + tokens[ix + 1:ix + window_offset + 1]
            windows[tokens[ix]] = curr_window

        return windows


class DocumentsStream(object):

    def __iter__(self):
        raise NotImplementedError()

    def get_batch(self, indexes: List[int]) -> List[DocumentModel]:
        raise NotImplementedError()


class DocumentsStreamInMemory(DocumentsStream):

    def __init__(self, documents: List[DocumentModel]):
        self.documents = documents

    def __iter__(self):
        return iter(self.documents)

    def get_batch(self, indexes: List[int]):
        return [self.documents[index] for index in indexes]


class DocumentsStreamSource(DocumentsStream):

    def __init__(self, source, options):
        self.source = source
        self.options = options

    def __iter__(self):
        document_id: int = 0

        while True:
            document = self.source.GetDocumentAsync(document_id).GetAwaiter().GetResult()
            document_id += 1

            if document.HasData is False:
                break

            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content, patterns=self.options.TokenizeRegex)
                yield DocumentModel(doc_id=paragraph.GlobalId, tokens=words)

    def get_batch(self, indexes: List[int]):
        batch = self.source.GetBatchAsync(indexes).GetAwaiter().GetResult()

        for document in batch:
            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content, patterns=self.options.TokenizeRegex)
                yield DocumentModel(doc_id=paragraph.GlobalId, tokens=words)


class DocumentVocab(object):
    _UNKNOWN: str = '<unk>'
    _NAME: str = 'vocab'

    def __init__(self, word2index, index2word, documents_count):
        self.word2index = word2index
        self.index2word = index2word
        self.documents_count = documents_count
        self.vocab_size = len(word2index)

    @staticmethod
    def build_from_stream(stream: DocumentsStream):
        current = 0
        documents_count = 0

        temp: Dict[str, int] = {}
        word2index: Dict[str, int] = {}
        index2word: Dict[int, str] = {}

        for document in stream:
            documents_count += 1

            for token in document.tokens:
                if token not in temp:
                    temp[token] = 1
                else:
                    temp[token] += 1

        for item in sorted(temp.items(), key=lambda x: x[1], reverse=True):
            token = item[0]
            word2index[token] = current
            index2word[current] = token
            current += 1

        word2index[DocumentVocab._UNKNOWN] = len(word2index)
        index2word[len(word2index)] = DocumentVocab._UNKNOWN

        return DocumentVocab(word2index, index2word, documents_count)

    @staticmethod
    def load(base_path):
        path = os.path.join(base_path, DocumentVocab._NAME)
        
        with open(path, 'rb') as f:
            load_data = pickle.load(f) or {}
        
        word2index = load_data['word2index']
        documents_count = load_data['documents_count']
        index2word: Dict[int, str] = {}
        
        for item in word2index.items():
            index2word[item[1]] = item[0]
            
        return DocumentVocab(word2index, index2word, documents_count)
        
    def to_index(self, token: str) -> int:
        return self.word2index.get(token, self.word2index[DocumentVocab._UNKNOWN])

    def to_token(self, index: int) -> str:
        return self.index2word[index]

    def save(self, base_path):
        path = os.path.join(base_path, DocumentVocab._NAME)
        
        with open(path, 'wb') as f:
            save_data = {
                'documents_count': self.documents_count,
                'word2index': self.word2index
            }
            
            pickle.dump(save_data, f)


class DocumentsGenerator(keras.utils.Sequence):

    def __init__(self,
                 stream: DocumentsStream,
                 vocab: DocumentVocab,
                 documents_count: int,
                 window_size: int,
                 batch_size: int,
                 shuffle: bool = True):

        self.stream = stream
        self.vocab = vocab
        self.window_size = window_size
        self.batch_size = batch_size
        self.shuffle = shuffle
        self.documents_count = documents_count
        self.indexes = list(range(documents_count))

    def get_infer_generator(self, infer_doc: DocumentModel):
        stream = DocumentsStreamInMemory([infer_doc])
        return DocumentsGenerator(stream, self.vocab, 1, self.window_size, 1)

    def encode_document(self, document: DocumentModel):
        v = self.vocab
        documents = []
        words = []
        outputs = []

        for window in document.generate_windows(window_size=self.window_size).items():
            enc_words = [v.to_index(word) for word in window[1]]
            enc_result = v.to_index(window[0])
            documents.append(document.doc_id)
            words.append(enc_words)
            outputs.append(to_categorical(enc_result, num_classes=v.vocab_size))

        return np.vstack(documents), words, np.vstack(outputs)

    def __len__(self):
        return int(self.documents_count / self.batch_size)

    def __getitem__(self, index):
        indexes = self.indexes[index * self.batch_size:(index + 1) * self.batch_size]
        documents = self.stream.get_batch(indexes)
        inputs, outputs = self.__data_generation(documents)
        return inputs, outputs

    def on_epoch_end(self):
        if self.shuffle:
            np.random.shuffle(self.indexes)

    def __data_generation(self, docs: List[DocumentModel]):
        batch_docs = []
        batch_words = []
        batch_outputs = []

        for doc in docs:
            enc_doc, enc_words, outputs = self.encode_document(doc)
            batch_docs.append(enc_doc)
            batch_words.extend(enc_words)
            batch_outputs.append(outputs)

        inputs = (np.vstack(batch_docs), np.vstack(batch_words))
        outputs = np.vstack(batch_outputs)
        return inputs, outputs


class KerasDoc2VecModel(object):
    def __init__(self, vocab: DocumentVocab, options, stream: DocumentsStream = None, model=None):
        self.options = options
        self.embedding_size = options.EmbeddingSize
        self.window_size = options.WindowSize
        self.vocab = vocab
        self.stream = stream
        self.model = model
        self.infer_model = None
        self.generator =\
            DocumentsGenerator(stream, self.vocab, self.vocab.documents_count, self.window_size, options.BatchSize)
        if model is not None:
            self.__retrieve_embeddings(self.model)

    def build(self, is_infer: bool = False):
        words_input = Input(shape=(self.window_size - 1,), name=InputWordsLayerName)
        document_id_input = Input(shape=(1,), name=InputDocumentsLayerName)

        document_inference = Embedding(input_dim=1,
                                       output_dim=self.embedding_size,
                                       input_shape=(1,),
                                       name=InferredDocumentsLayerName,
                                       embeddings_initializer="uniform")(document_id_input)

        words_embedding = Embedding(input_dim=self.vocab.vocab_size,
                                    output_dim=self.embedding_size,
                                    input_shape=(self.window_size - 1,),
                                    name=WordEmbeddingsLayerName)(words_input)

        document_embedding = Embedding(input_dim=self.vocab.documents_count,
                                       output_dim=self.embedding_size,
                                       input_shape=(1,),
                                       name=DocumentEmbeddingsLayerName)(document_id_input)

        merged = self._concatenate_layers(document_inference, document_embedding, words_embedding, is_infer)

        squeezed = self.__create_hidden_layers(merged, self.options.Layers)

        output = Dense(self.vocab.vocab_size, activation='softmax', name=OutputLayerName)(squeezed)

        model = Model(inputs=[document_id_input, words_input], outputs=output)

        self._update_main_model(model, is_infer)

    def train(self, epochs: int, learning_rate=0.1, verbose: int = 0):
        metrics = list(self.options.Metrics)
        callback_loss = EarlyStopping(monitor='loss', patience=7, verbose=1, min_delta=0.01)
        start_time = time.time()
        optimizer = Adam(learning_rate=learning_rate)
        self.model.compile(loss=self.options.Loss, optimizer=optimizer, metrics=metrics)
        history = self.model.fit(self.generator,
                                 epochs=epochs,
                                 verbose=verbose,
                                 callbacks=[callback_loss])
        self.__retrieve_embeddings(self.model)
        train_time = time.time() - start_time
        
        if self.options.IsPlotResults:
            KerasDoc2VecModel.plot_history(history, metrics, self.options.Name, self.vocab.documents_count, train_time)

    def infer_vector(self, infer_document: DocumentModel,
                     n: int = 5,
                     epochs: int = 5,
                     learning_rate: int = 0.1,
                     verbose: int = 0):

        if self.infer_model is None:
            self.build(is_infer=True)
        else:
            new_weights = np.random.uniform(-0.05, 0.05, self.embedding_size).reshape(1, -1)
            self.__get_infer_documents_layer().set_weights([new_weights])

        generator = self.generator.get_infer_generator(infer_document)

        for layer in self.infer_model.layers:
            if layer.name not in [InferredDocumentsLayerName, InputDocumentsLayerName]:
                train_weights = self.model.get_layer(layer.name).get_weights()
                layer.set_weights(train_weights)

        for layer in self.infer_model.layers:
            layer.trainable = False

        self.__get_infer_documents_layer().trainable = True

        optimizer = Adam(learning_rate=learning_rate)
        early_stop_callback = EarlyStopping(monitor='loss', patience=7, verbose=1, min_delta=0.01)
        metrics = list(self.options.Metrics)
        self.infer_model.compile(loss=self.options.Loss, optimizer=optimizer, metrics=metrics)
        self.infer_model.fit(generator, epochs=epochs, verbose=verbose, callbacks=[early_stop_callback])
        inferred_vector = self.__get_infer_documents_layer().get_weights()[0]
        return self.top_n(inferred_vector, n)

    def save(self, path):
        self.model.save(path)

    def get_document_embeddings(self):
        return self.document_embeddings

    def get_word_embeddings(self):
        return self.word_embeddings

    def top_n(self, inferred_vector, n: int) -> List[InferenceData]:
        embeddings = self.get_document_embeddings()
        result: List[InferenceData] = []

        for i in range(n):
            result.append(InferenceData(0, 0))

        current = 0
        inferred_vector = inferred_vector.reshape(1, -1)
        is_sorted = False

        for i, embed in enumerate(embeddings):
            cos = cosine_similarity(inferred_vector, embed.reshape(1, -1))[0][0]

            if current < n:
                result[current] = InferenceData(cos, i)
                current += 1
            else:
                if is_sorted is False:
                    result = sorted(result, key=lambda x: x.cos)
                    is_sorted = True

                for j in range(n - 1, -1, -1):
                    if cos > result[j].cos:
                        result[j] = InferenceData(cos, i)
                        break

        return result
    
    @staticmethod
    def plot_history(history, metrics, name, documents, train_time):
        epochs_range = range(len(history.history[metrics[0]]))
        
        for metric in metrics:
            plt.plot(epochs_range, history.history[metric], label=metric)
            
        plt.title('{0}. Documents: {1}. Time: {2} s'.format(name, documents, train_time))
        plt.xlabel('Epoch')
        plt.legend(loc='best')
        plt.show()

    def __get_infer_documents_layer(self):
        return self.infer_model.get_layer(InferredDocumentsLayerName)

    def __retrieve_embeddings(self, model: Model):
        self.document_embeddings = model.get_layer(DocumentEmbeddingsLayerName).get_weights()[0]
        self.word_embeddings = model.get_layer(WordEmbeddingsLayerName).get_weights()[0]

    def _update_main_model(self, model: Model, is_infer: bool):
        if is_infer:
            self.infer_model = model
        else:
            self.model = model

    def __create_hidden_layers(self, merged, options):
        last_layer = merged

        for layer in options:
            current_layer = None

            if layer.Type == "Dropout":
                current_layer = Dropout(rate=layer.GetInt("Rate"), name=layer.Name)
            elif layer.Type == "Average":
                current_layer = Average(name=layer.Name)
            elif layer.Type == "Flatten":
                current_layer = Flatten(name=layer.Name)
            elif layer.Type == "LSTM":
                current_layer = LSTM(units=layer.GetInt("UnitsCount"),
                                     return_sequences=layer.GetBool("ReturnSequences"),
                                     name=layer.Name)
            elif layer.Type == "GRU":
                current_layer = GRU(units=layer.GetInt("UnitsCount"),
                                    return_sequences=layer.GetBool("ReturnSequences"),
                                    name=layer.Name)
            elif layer.Type == "Lambda":
                lambda_name = layer.GetString("LambdaName")

                if lambda_name == "split":
                    current_layer = Lambda(split_tensor(self.window_size - 1), name=layer.Name)
                elif lambda_name == "squeeze":
                    current_layer = Lambda(squeeze_tensor(axis=1), name=layer.Name)

            last_layer = current_layer(last_layer)

        return last_layer

    @staticmethod
    def _concatenate_layers(on_infer: Layer, on_train: Layer, shared: Layer, is_infer: bool):
        if is_infer:
            return Concatenate(name=MergedLayerName, axis=1)([on_infer, shared])
        else:
            return Concatenate(name=MergedLayerName, axis=1)([on_train, shared])


def split_tensor(window_size):
    def _lambda(tensor):
        return tf.split(tensor, window_size + 1, axis=1)

    return _lambda


def squeeze_tensor(axis=-1):
    def _lambda(tensor):
        return tf.squeeze(tensor, axis=axis)

    return _lambda


def stack_tensor(window_size):
    def _lambda(tensor):
        return tf.stack([tensor] * window_size, axis=1)

    return _lambda
