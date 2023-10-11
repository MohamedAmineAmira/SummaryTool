import spacy
from nltk.tokenize import sent_tokenize, word_tokenize
from nltk.corpus import stopwords

def cleaning_french_text(french_text):
    # Tokenize the text into sentences
    sentences = sent_tokenize(french_text, language='french')

    # Tokenize each sentence into words
    tokenized_sentences = [word_tokenize(sentence, language='french') for sentence in sentences]

    # Remove stop words from each tokenized sentence
    filtered_sentences = []
    for sentence in tokenized_sentences:
        filtered_sentence = [word for word in sentence if word.lower() not in stop_words_french]
        filtered_sentences.append(filtered_sentence)

    # lemmatizated each sentence
    lemmatized_sentences = []
    for sentence in filtered_sentences:
        doc = nlp_french(" ".join(sentence))
        lemmatized_sentence = [token.lemma_ for token in doc]
        lemmatized_sentences.append(lemmatized_sentence)

    # Rejoin tokenized sentences into clean text
    cleaned_text = [" ".join(sentence) for sentence in lemmatized_sentences]
    formatted_text = ''.join(cleaned_text)
    return formatted_text

# Load French stop words
stop_words_french = set(stopwords.words('french'))

# Perform lemmatization using a library like spaCy or pattern.fr (not available in NLTK)
# load with: python -m spacy download fr_core_news_sm
nlp_french = spacy.load('fr_core_news_sm')

