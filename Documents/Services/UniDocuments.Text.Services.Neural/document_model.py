import re
import nltk

from gensim.models.doc2vec import Doc2Vec, TaggedDocument
from nltk.corpus import stopwords

nltk.download('stopwords')
sw = stopwords.words('russian')


def preprocess_and_tokenize(text, patterns="[0-9!#$%&'()*+,./:;<=>?@[\\]^_`{|}~\"\\-−]+"):
    doc = re.sub(patterns, ' ', text).lower()
    tokens = []
    for token in doc.split():
        if token and token not in sw:
            token = token.strip()
            tokens.append(token)
    return tokens


class DocumentStream(object):
    def __init__(self, getter, data_handler):
        self.getter = getter
        self.data_handler = data_handler

    def __iter__(self):
        paragraph_id = -1
        self.getter.InitializeAsync().GetAwaiter().GetResult()
        
        while True:
            document = self.getter.GetNextDocumentAsync().GetAwaiter().GetResult()
            
            if document.HasData is False:
                self.getter.Dispose()
                break
            
            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content)
                
                if len(words) <= 10:
                    continue
                    
                paragraph_id += 1
                paragraph.Id = paragraph_id
                self.data_handler.OnTrainDataSetup(document, paragraph)
                yield TaggedDocument(words=words, tags=[paragraph_id])


def train(source, data_handler):
    tagged_documents = DocumentStream(source, data_handler)
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
    data_handler.OnTrainComplete()
    return model


def load(path):
    return Doc2Vec.load(path)


def infer(model, text):
    vec = model.infer_vector(preprocess_and_tokenize(text))
    return model.docvecs.most_similar(vec, topn=5)
