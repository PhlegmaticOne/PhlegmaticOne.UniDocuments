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
            if len(token) > 1:
                tokens.append(token)
    return tokens


class DocumentStream(object):
    def __init__(self, source, options):
        self.source = source
        self.options = options

    def __iter__(self):
        self.source.InitializeAsync().GetAwaiter().GetResult()
        
        while True:
            document = self.source.GetNextDocumentAsync().GetAwaiter().GetResult()
            
            if document.HasData is False:
                self.source.Dispose()
                break
            
            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content, patterns=self.options.TokenizeRegex)
                yield TaggedDocument(words=words, tags=[paragraph.GlobalId])


def train(input_data):
    options = input_data.Options
    source = input_data.Source

    tagged_documents = DocumentStream(source, options)

    model = Doc2Vec(vector_size=options.VectorSize,
                    alpha=options.Alpha,
                    min_alpha=options.MinAlpha,
                    min_count=options.MinWordsCount,
                    dm=options.Dm,
                    epochs=options.Epochs,
                    workers=options.WorkersCount)

    model.build_vocab(tagged_documents)
    model.train(tagged_documents, total_examples=model.corpus_count, epochs=model.epochs)
    return model


def load(input_data):
    return Doc2Vec.load(input_data)


def save(input_data):
    model = input_data.Model
    path = input_data.Path
    model.save(path)
    

def infer(input_data):
    model = input_data.Model
    text = input_data.Content
    options = input_data.Options
    top_n = input_data.TopN
    
    preprocess = preprocess_and_tokenize(text, patterns=options.TokenizeRegex)
    vec = model.infer_vector(preprocess)
    return model.docvecs.most_similar(vec, topn=top_n)
