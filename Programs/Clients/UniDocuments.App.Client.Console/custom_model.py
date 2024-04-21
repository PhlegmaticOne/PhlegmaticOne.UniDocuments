from keras.models import Model
from keras.layers import Layer, Embedding, Input, Concatenate, LSTM, Dense, Lambda, Average, Flatten
from keras.callbacks import History, EarlyStopping
from keras.optimizers import Adam
from keras.utils import to_categorical
import numpy as np
import tensorflow as tf
import keras
import nltk
from nltk.corpus import stopwords
import re
from collections.abc import Iterator
from typing import List, Dict

InputWordsLayerName = 'input_words'
InputDocumentsLayerName = 'input_documents'
InferredDocumentsLayerName = 'inferred_documents'
WordEmbeddingsLayerName = 'word_embeddings'
DocumentEmbeddingsLayerName = 'document_embeddings'
MergedLayerName = 'merged'
LstmLayerName = 'lstm'
GruLayerName = 'gru'
OutputLayerName = 'output'

nltk.download('stopwords')
sw = stopwords.words('russian')


class Document(object):

    def __init__(self, doc_id: int, tokens: List[str]):
        self.tokens = tokens
        self.doc_id = doc_id

    def generate_windows(self, window_size: int) -> Dict[str, List[str]]:
        windows: Dict[str, List[str]] = {}

        if window_size % 2 != 1:
            raise ValueError("Parameter window_size must be an odd number.")

        tokens = self.tokens
        window_offset = (window_size - 1) // 2
        words_count = len(tokens)

        for ix in range(window_offset, words_count - window_offset):
            curr_window = tokens[ix - window_offset:ix] + tokens[ix + 1:ix + window_offset + 1]
            windows[tokens[ix]] = curr_window

        return windows


class DocumentsStream(object):

    def __iter__(self) -> Iterator[Document]:
        raise NotImplementedError()

    def get_batch(self, indexes: List[int]) -> List[Document]:
        raise NotImplementedError()


class DocumentsStreamInMemory(DocumentsStream):

    def __init__(self, documents: List[Document]):
        self.documents = documents

    def __iter__(self) -> Iterator[Document]:
        return iter(self.documents)

    def get_batch(self, indexes: List[int]):
        return [self.documents[index] for index in indexes]


class DocumentsStreamGenerator(DocumentsStream):

    def __init__(self, generator, options):
        self.generator = generator
        self.options = options

    def __iter__(self) -> Iterator[Document]:
        self.generator.InitializeAsync().GetAwaiter().GetResult()

        while True:
            document = self.generator.GetNextDocumentAsync().GetAwaiter().GetResult()

            if document.HasData is False:
                self.generator.Dispose()
                break

            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content, patterns=self.options.TokenizeRegex)
                yield Document(doc_id=paragraph.GlobalId, tokens=words)

    def get_batch(self, indexes: List[int]):
        return self.generator.GetAtIndexesAsync()


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
                 batch_size: int = 100,
                 shuffle: bool = True):

        self.stream = stream
        self.vocab = vocab
        self.window_size = window_size
        self.batch_size = batch_size
        self.shuffle = shuffle
        self.documents_count = documents_count
        self.indexes = list(range(documents_count))

    def get_infer_generator(self, infer_doc: Document):
        stream = DocumentsStreamInMemory([infer_doc])
        return DocumentsGenerator(stream, self.vocab, 1, self.window_size, 1)

    def encode_document(self, document: Document):
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

    def __data_generation(self, docs: List[Document]):
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

    def __init__(self,
                 stream: DocumentsStream,
                 embedding_size: int = 16,
                 window_size: int = 3,
                 batch_size: int = 2):

        self.embedding_size = embedding_size
        self.window_size = window_size
        self.batch_size = batch_size
        self.vocab = DocumentVocab(stream)
        self.stream = stream
        self.model = self.infer_model = None
        self.generator = DocumentsGenerator(stream, self.vocab, self.vocab.documents_count, window_size, batch_size)

    def build(self, is_infer: bool = False):
        raise NotImplementedError()

    def train(self, epochs: int, learning_rate=0.1, verbose=0) -> History:
        es = EarlyStopping(monitor='loss', mode='min', patience=10, verbose=1)
        optimizer = Adam(learning_rate=learning_rate)
        self.model.compile(loss="categorical_crossentropy", optimizer=optimizer, metrics=['accuracy'])
        history = self.model.fit(self.generator, epochs=epochs, verbose=verbose, callbacks=[es])
        self._retrieve_embeddings(self.model)
        return history

    def infer_vector(self, infer_document: Document, epochs: int = 5, learning_rate: int = 0.1, verbose: int = 0):
        if self.infer_model is None:
            self.build(is_infer=True)
        else:
            infer_doc_layer = self.infer_model.get_layer(InferredDocumentsLayerName)
            w = np.random.uniform(-0.05, 0.05, self.embedding_size).reshape(1, -1)
            infer_doc_layer.set_weights([w])

        generator = self.generator.get_infer_generator(infer_document)

        for layer in self.infer_model.layers:
            if layer.name not in [InferredDocumentsLayerName, InputDocumentsLayerName]:
                train_weights = self.model.get_layer(layer.name).get_weights()
                layer.set_weights(train_weights)

        for layer in self.infer_model.layers:
            layer.trainable = False

        self.infer_model.get_layer(InferredDocumentsLayerName).trainable = True

        optimizer = Adam(learning_rate=learning_rate)
        self.infer_model.compile(loss="categorical_crossentropy", optimizer=optimizer, metrics=['accuracy'])

        es = EarlyStopping(monitor='loss', mode='min', patience=10, verbose=1)
        self.infer_model.fit(generator, epochs=epochs, verbose=verbose, callbacks=[es])

        w = self.infer_model.get_layer(InferredDocumentsLayerName).get_weights()
        return w[0]

    def get_document_embeddings(self):
        return self.document_embeddings

    def get_word_embeddings(self):
        return self.word_embeddings

    def _retrieve_embeddings(self, model: Model):
        self.document_embeddings = model.get_layer(DocumentEmbeddingsLayerName).get_weights()[0]
        self.word_embeddings = model.get_layer(WordEmbeddingsLayerName).get_weights()[0]

    def _update_main_model(self, model: Model, is_infer: bool):
        if is_infer:
            self.infer_model = model
        else:
            self.model = model

    @staticmethod
    def _concatenate_layers(on_infer: Layer, on_train: Layer, shared: Layer, is_infer: bool):
        if is_infer:
            return Concatenate(name=MergedLayerName, axis=1)([on_infer, shared])
        else:
            return Concatenate(name=MergedLayerName, axis=1)([on_train, shared])


class Doc2VecModelLSTM(Doc2VecModel):

    def build(self, is_infer: bool = False):
        words_input = Input(shape=(self.window_size - 1,), name=InputWordsLayerName)
        document_id_input = Input(shape=(1,), name=InputDocumentsLayerName)

        document_inference = Embedding(input_dim=1,
                                       output_dim=self.embedding_size,
                                       input_shape=1,
                                       name=InferredDocumentsLayerName,
                                       embeddings_initializer="uniform")(document_id_input)

        words_embedding = Embedding(input_dim=self.vocab.vocab_size,
                                    output_dim=self.embedding_size,
                                    input_shape=self.window_size - 1,
                                    name=WordEmbeddingsLayerName)(words_input)

        document_embedding = Embedding(input_dim=self.vocab.documents_count,
                                       output_dim=self.embedding_size,
                                       input_shape=1,
                                       name=DocumentEmbeddingsLayerName)(document_id_input)

        merged = self._concatenate_layers(document_inference, document_embedding, words_embedding, is_infer)

        lstm = LSTM(200, name=LstmLayerName)(merged)

        output = Dense(self.vocab.vocab_size, activation='softmax', name=OutputLayerName)(lstm)

        model = Model(inputs=[document_id_input, words_input], outputs=output)

        self._update_main_model(model, is_infer)


class Doc2VecModelOriginal(Doc2VecModel):

    def build(self, is_infer: bool = False):
        words_input = Input(shape=(self.window_size - 1,), name=InputWordsLayerName)
        document_id_input = Input(shape=(1,), name=InputDocumentsLayerName)

        document_inference = Embedding(input_dim=1,
                                       output_dim=self.embedding_size,
                                       input_shape=1,
                                       name=InferredDocumentsLayerName,
                                       embeddings_initializer="uniform")(document_id_input)

        words_embedding = Embedding(input_dim=self.vocab.vocab_size,
                                    output_dim=self.embedding_size,
                                    input_shape=self.window_size - 1,
                                    name=WordEmbeddingsLayerName)(words_input)

        document_embedding = Embedding(input_dim=self.vocab.documents_count,
                                       output_dim=self.embedding_size,
                                       input_shape=1,
                                       name=DocumentEmbeddingsLayerName)(document_id_input)

        merged = self._concatenate_layers(document_inference, document_embedding, words_embedding, is_infer)

        split = Lambda(split_w(self.window_size - 1), name='split')(merged)
        averaged = Average(name='average')(split)
        squeezed = Lambda(squeeze(axis=1), name='squeeze')(averaged)

        output = Dense(self.vocab.vocab_size, activation='softmax', name=OutputLayerName)(squeezed)

        model = Model(inputs=[document_id_input, words_input], outputs=output)

        self._update_main_model(model, is_infer)


def preprocess_and_tokenize(text, patterns="[0-9!#$%&'()*+,./:;<=>?@[\\]^_`{|}~\"\\-−]+"):
    doc = re.sub(patterns, ' ', text).lower()
    tokens = []
    for token in doc.split():
        if token and token not in sw:
            token = token.strip()
            if len(token) > 1:
                tokens.append(token)
    return tokens


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
