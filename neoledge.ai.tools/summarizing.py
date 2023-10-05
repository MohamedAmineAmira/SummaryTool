from transformers import AutoTokenizer, AutoModelForSeq2SeqLM
from transformers import pipeline
import torch


def summarizing_french_text(french_text):
    #model and tokenizer loading
    checkpoint = "venv/barthez-orangesum-abstract"
    tokenizer = AutoTokenizer.from_pretrained(checkpoint)
    base_model = AutoModelForSeq2SeqLM.from_pretrained(checkpoint, device_map='auto', torch_dtype=torch.float32)
    #LLM pipeline
    pipe_sum = pipeline(
        'summarization',
        model=base_model,
        tokenizer=tokenizer,
        max_length=256,
        min_length=50)

    result = pipe_sum(french_text)
    result = result[0]['summary_text']
    return result