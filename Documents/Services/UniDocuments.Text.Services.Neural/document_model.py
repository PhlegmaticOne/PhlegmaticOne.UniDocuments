import re
import nltk

from gensim.models.doc2vec import Doc2Vec, TaggedDocument
from nltk.corpus import stopwords

nltk.download('stopwords')
sw = stopwords.words('english')


def preprocess_and_tokenize(text, patterns="[0-9!#$%&'()*+,./:;<=>?@[\\]^_`{|}~—\"-]+"):
    doc = re.sub(patterns, ' ', text).lower()
    tokens = []
    for token in doc.split():
        if token and token not in sw:
            token = token.strip()
            tokens.append(token)
    return tokens


class DocumentStream(object):
    def __init__(self, getter):
        self.getter = getter

    def __iter__(self):
        document_number = -1
        paragraph_id = -1
        
        while True:
            document_number += 1
            data = self.getter.GetTrainDataAsync(document_number).GetAwaiter().GetResult()
            
            if data.HasData is False:
                break
            
            for paragraph in data.Paragraphs:
                paragraph_id += 1
                words = preprocess_and_tokenize(paragraph.Content)
                yield TaggedDocument(words=words, tags=[paragraph_id])


def train(getter):
    tagged_documents = DocumentStream(getter)
    max_epochs = 40
    vec_size = 50
    alpha = 0.025

    model = Doc2Vec(vector_size=vec_size,
                    alpha=alpha,
                    min_alpha=0.00025,
                    min_count=1,
                    dm=0,
                    epochs=max_epochs,
                    workers=4)

    model.build_vocab(tagged_documents)
    model.train(tagged_documents, total_examples=model.corpus_count, epochs=model.epochs)
    return model


def load(path):
    return Doc2Vec.load(path)


def infer(model, text):
    vec = model.infer_vector(preprocess_and_tokenize(text))
    return model.docvecs.most_similar(vec, topn=5)
