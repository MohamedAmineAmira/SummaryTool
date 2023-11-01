# SummaryTool

## Download French Model (BARThez)

You can download the French model from [this link](https://huggingface.co/moussaKam/barthez-orangesum-abstract/tree/main).

## Download english Model (LaMini-Flan-T5-248M)

You can download the English model from [this link](https://huggingface.co/MBZUAI/LaMini-Flan-T5-248M/tree/main).

# AI Tool

This tool utilizes several libraries and dependencies. To get started, make sure you have the following installed:

- Flask
- NLTK
- SpaCy
- Accelerate
- PyTorch
- Transformers
- SentencePiece
- SafeTensors

You can install these dependencies using pip:

```bash
pip install flask nltk spacy accelerate torch transformers sentencepiece safetensors
```

Additionally, you'll need to download the language models for SpaCy. Run the following commands:

```bash
python -m spacy download en_core_web_sm
python -m spacy download fr_core_news_sm

