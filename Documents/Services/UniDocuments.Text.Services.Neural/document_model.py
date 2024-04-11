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
    def __init__(self, getter, options, data_handler):
        self.getter = getter
        self.data_handler = data_handler
        self.options = options

    def __iter__(self):
        paragraph_id = -1
        self.getter.InitializeAsync().GetAwaiter().GetResult()
        
        while True:
            document = self.getter.GetNextDocumentAsync().GetAwaiter().GetResult()
            
            if document.HasData is False:
                self.getter.Dispose()
                break
            
            for paragraph in document.Paragraphs:
                words = preprocess_and_tokenize(paragraph.Content, patterns=self.options.TokenizeRegex)
                
                if len(words) <= self.options.ParagraphMinWordsCount:
                    continue
                    
                paragraph_id += 1
                paragraph.Id = paragraph_id
                self.data_handler.OnTrainDataSetup(document, paragraph)
                yield TaggedDocument(words=words, tags=[paragraph_id])


def train(source, options, data_handler):
    
    tagged_documents = DocumentStream(source, options, data_handler)

    model = Doc2Vec(vector_size=options.VectorSize,
                    alpha=options.Alpha,
                    min_alpha=options.MinAlpha,
                    min_count=options.MinWordsCount,
                    dm=options.Dm,
                    epochs=options.Epochs,
                    workers=options.WorkersCount)

    model.build_vocab(tagged_documents)
    model.train(tagged_documents, total_examples=model.corpus_count, epochs=model.epochs)
    data_handler.OnTrainComplete()
    return model


def load(path):
    return Doc2Vec.load(path)


def infer(model, options, text, top_n):
    preprocess = preprocess_and_tokenize(text, patterns=options.TokenizeRegex)
    
    if len(preprocess) < options.ParagraphMinWordsCount:
        return None
    
    vec = model.infer_vector(preprocess)
    return model.docvecs.most_similar(vec, topn=top_n)
