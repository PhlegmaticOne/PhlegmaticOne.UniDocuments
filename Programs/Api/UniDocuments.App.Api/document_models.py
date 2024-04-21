from gensim.models.doc2vec import Doc2Vec, TaggedDocument
from keras.models import Model, load_model
from keras.layers import Layer, Embedding, Input, Concatenate, LSTM, Dense, Lambda, Average
from keras.callbacks import EarlyStopping
from keras.optimizers import Adam
from keras.utils import to_categorical
import numpy as np
import tensorflow as tf
import keras
import nltk
from nltk.corpus import stopwords
import re
from typing import List, Dict

InputWordsLayerName = 'input_words'
InputDocumentsLayerName = 'input_documents'
InferredDocumentsLayerName = 'inferred_documents'
WordEmbeddingsLayerName = 'word_embeddings'
DocumentEmbeddingsLayerName = 'document_embeddings'
MergedLayerName = 'merged'
OutputLayerName = 'output'

nltk.download('stopwords')
sw = stopwords.words('russian')


def train_doc2vec(input_data) -> Doc2Vec:
    options = input_data.Options
    source = input_data.Source

    tagged_documents = DocumentStream(source, options)

    model = Doc2Vec(vector_size=options.EmbeddingSize,
                    alpha=options.Alpha,
                    min_alpha=options.MinAlpha,
                    min_count=options.MinWordsCount,
                    dm=options.Dm,
                    epochs=options.Epochs,
                    workers=options.WorkersCount)

    model.build_vocab(tagged_documents)
    model.train(tagged_documents, total_examples=model.corpus_count, epochs=model.epochs)
    return model


def load_doc2vec(input_data) -> Doc2Vec:
    return Doc2Vec.load(input_data)


def save_doc2vec(input_data):
    model = input_data.Model
    path = input_data.Path
    model.save(path)


def infer_doc2vec(input_data):
    model = input_data.Model
    text = input_data.Content
    options = input_data.Options
    top_n = input_data.TopN

    preprocess = preprocess_and_tokenize(text, patterns=options.TokenizeRegex)
    vec = model.infer_vector(preprocess)
    return model.docvecs.most_similar(vec, topn=top_n)


def preprocess_and_tokenize(text: str, patterns: str = "[0-9!#$%&'()*+,./:;<=>?@[\\]^_`{|}~\"\\-−]+") -> List[str]:
    doc = re.sub(patterns, ' ', text).lower()
    tokens: List[str] = []
    for token in doc.split():
        if token and token not in sw:
            token = token.strip()
            if len(token) > 1:
                tokens.append(token)
    return tokens


class DocumentStream(object):
    def __init__(self, source, options):
        self.source = source
        self.options = options

    def __iter__(self):
        document_id: int = 0
        self.source.Initialize()

        while True:
            document = self.source.GetDocumentAsync(document_id).GetAwaiter().GetResult()
            document_id += 1

            if document.HasData is False:
                self.source.Dispose()
                break

            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content, patterns=self.options.TokenizeRegex)
                yield TaggedDocument(words=words, tags=[paragraph.GlobalId])


def train_custom(input_data):
    options = input_data.Options
    source = input_data.Source
    stream = DocumentsStreamSource(source, options)
    model = Doc2VecModel(stream, options)
    model.build(is_infer=False)
    model.train(options.Epochs, options.LearningRate, options.Verbose)
    return model


def save_custom(input_data):
    model = input_data.Model
    path = input_data.Path
    model.save(path)


def load_custom(input_data):
    return load_model(input_data)


def split_w(window_size):
    def _lambda(tensor):
        return tf.split(tensor, window_size + 1, axis=1)

    return _lambda


def squeeze(axis=-1):
    def _lambda(tensor):
        return tf.squeeze(tensor, axis=axis)

    return _lambda


def stack(window_size):
    def _lambda(tensor):
        return tf.stack([tensor] * window_size, axis=1)

    return _lambda


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
        self.source.Initialize()

        while True:
            document = self.source.GetDocumentAsync(document_id).GetAwaiter().GetResult()
            document_id += 1

            if document.HasData is False:
                self.source.Dispose()
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

    def __init__(self, stream: DocumentsStream):
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

        self.word2index = word2index
        self.index2word = index2word
        self.documents_count = documents_count
        self.vocab_size = len(index2word)

    def to_index(self, token: str) -> int:
        return self.word2index[token]

    def to_token(self, index: int) -> str:
        return self.index2word[index]


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


class Doc2VecModel(object):
    def __init__(self, stream: DocumentsStream, options):
        self.options = options
        self.embedding_size = options.EmbeddingSize
        self.window_size = options.WindowSize
        self.vocab = DocumentVocab(stream)
        self.stream = stream
        self.model = self.infer_model = None
        self.generator = DocumentsGenerator(stream, self.vocab, self.vocab.documents_count,
                                            self.window_size, options.BatchSize)

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

        merged = self.__concatenate_layers(document_inference, document_embedding, words_embedding, is_infer)

        last_hidden_layer = self.__create_hidden_layers(merged, self.options.Layers)

        output = Dense(self.vocab.vocab_size, activation='softmax', name=OutputLayerName)(last_hidden_layer)

        model = Model(inputs=[document_id_input, words_input], outputs=output)

        self.__update_main_model(model, is_infer)

    def train(self, epochs: int, learning_rate=0.1, verbose: int = 0):
        early_stop_callback = EarlyStopping(monitor='loss', mode='min', patience=10, verbose=1)
        optimizer = Adam(learning_rate=learning_rate)
        self.model.compile(loss="categorical_crossentropy", optimizer=optimizer, metrics=['accuracy'])
        self.model.fit(self.generator, epochs=epochs, verbose=verbose, callbacks=[early_stop_callback])
        self.__retrieve_embeddings(self.model)

    def infer_vector(self, infer_document: DocumentModel, epochs: int = 5, learning_rate: int = 0.1, verbose: int = 0):
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
        early_stop_callback = EarlyStopping(monitor='loss', mode='min', patience=10, verbose=1)
        self.infer_model.compile(loss="categorical_crossentropy", optimizer=optimizer, metrics=['accuracy'])
        self.infer_model.fit(generator, epochs=epochs, verbose=verbose, callbacks=[early_stop_callback])
        return self.__get_infer_documents_layer().get_weights()[0]

    def save(self, path):
        self.model.save(path)

    def get_document_embeddings(self):
        return self.document_embeddings

    def get_word_embeddings(self):
        return self.word_embeddings

    def __get_infer_documents_layer(self):
        return self.infer_model.get_layer(InferredDocumentsLayerName)

    def __retrieve_embeddings(self, model: Model):
        self.document_embeddings = model.get_layer(DocumentEmbeddingsLayerName).get_weights()[0]
        self.word_embeddings = model.get_layer(WordEmbeddingsLayerName).get_weights()[0]

    def __update_main_model(self, model: Model, is_infer: bool):
        if is_infer:
            self.infer_model = model
        else:
            self.model = model

    def __create_hidden_layers(self, merged, options):
        last_layer = merged

        for layer in options:
            # noinspection PyTypeChecker
            current_layer: Layer = None

            if layer.Type == "LSTM":
                current_layer = LSTM(units=layer.GetInt("UnitsCount"),
                                     return_sequences=layer.GetBool("ReturnSequences"),
                                     name=layer.Name)
            elif layer.Type == "Average":
                current_layer = Average(name=layer.Name)
            elif layer.Type == "Lambda":
                lambda_name = layer.GetString("LambdaName")

                if lambda_name == "squeeze":
                    current_layer = Lambda(split_w(self.window_size - 1), name=layer.name)
                elif lambda_name == "split":
                    current_layer = Lambda(squeeze(axis=1), name=layer.name)

            last_layer = current_layer(last_layer)

        return last_layer

    @staticmethod
    def __concatenate_layers(on_infer: Layer, on_train: Layer, shared: Layer, is_infer: bool):
        if is_infer:
            return Concatenate(name=MergedLayerName, axis=1)([on_infer, shared])
        else:
            return Concatenate(name=MergedLayerName, axis=1)([on_train, shared])
