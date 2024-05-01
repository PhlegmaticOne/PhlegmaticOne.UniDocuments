import os.path
import re
from typing import List
from gensim.models.doc2vec import Doc2Vec, TaggedDocument
import nltk
from nltk.corpus import stopwords
from pymorphy2 import MorphAnalyzer

InputWordsLayerName = 'input_words'
InputDocumentsLayerName = 'input_documents'
InferredDocumentsLayerName = 'inferred_documents'
WordEmbeddingsLayerName = 'word_embeddings'
DocumentEmbeddingsLayerName = 'document_embeddings'
MergedLayerName = 'merged'
OutputLayerName = 'output'

nltk.download('stopwords')
sw = stopwords.words('russian')
analyzer = MorphAnalyzer()


def preprocess_text(input_data):
    text = input_data.Text
    patterns = input_data.TokenizeRegex
    tokenized = preprocess_and_tokenize(text, patterns)
    return " ".join(tokenized)


def preprocess_and_tokenize(text: str, patterns: str = "[0-9!#$%&'()*+,./:;<=>?@[\\]^_`{|}~\"\\-−]+") -> List[str]:
    doc = re.sub(patterns, ' ', text).lower()
    tokens: List[str] = []
    for token in doc.split():
        if token and token not in sw:
            token = token.strip()
            token = analyzer.normal_forms(token)[0]
            if len(token) > 1:
                tokens.append(token)
    return tokens


class DocumentStream(object):
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
                yield TaggedDocument(words=words, tags=[paragraph.GlobalId])
                
                
def train(input_data) -> Doc2Vec:
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


def load(input_data) -> Doc2Vec:
    path = get_path(input_data)
    return Doc2Vec.load(path)


def save(input_data):
    model = input_data.Model
    path = get_path(input_data.Path)
    model.save(path)


def infer(input_data):
    model = input_data.Model
    text = input_data.Content
    options = input_data.Options
    top_n = input_data.TopN

    preprocess = preprocess_and_tokenize(text, patterns=options.TokenizeRegex)
    infer_vector = model.infer_vector(preprocess)
    return model.docvecs.most_similar(infer_vector, topn=top_n)


def get_path(input_data):
    name = "{0}.bin".format(input_data.Name)
    base_path = input_data.BasePath
    return os.path.join(base_path, name)
