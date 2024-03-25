import nltk
import re
from nltk.corpus import stopwords
from pymorphy2 import MorphAnalyzer


class TextPreprocessor(object):

    def __init__(self):
        nltk.download('stopwords')
        self.stopwords = stopwords.words("russian")
        self.morph = MorphAnalyzer()
        self.patterns = "[A-Za-z0-9!#$%&'()*+,./:;<=>?@[]^_`{|}~�\"-]+"

    def preprocess_text(self, text: str) -> str:

        doc = re.sub(self.patterns, ' ', text)
        tokens = []

        for token in doc.split():
            if token and token not in self.stopwords:
                token = token.strip()
                token = self.morph.normal_forms(token)[0]
                tokens.append(token)

        return " ".join(tokens)
