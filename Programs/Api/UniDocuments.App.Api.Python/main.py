from fastapi import FastAPI, UploadFile
from pydantic import BaseModel
import uvicorn
from text_preprocessor import TextPreprocessor
from docx import Document
from io import BytesIO


class PreprocessQuery(BaseModel):
    text: str


class PreprocessResult(BaseModel):
    text: str


app = FastAPI()
textPreprocessor = TextPreprocessor()


@app.get("/")
async def root():
    return {"Hello": "world"}


@app.post("/preprocess/")
async def preprocess(query: PreprocessQuery) -> PreprocessResult:
    processed = textPreprocessor.preprocess_text(query.text)
    return PreprocessResult(text=processed)

@app.post("/uploadfile/")
async def upload(file: UploadFile) -> list[str]:
    file_bytes = BytesIO(await file.read())
    document = Document(docx=file_bytes)
    
    result = []
    
    for p in document.paragraphs:
        text = p.text
        
        if text:
            result.append(p.text)            
        
    return result

if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000)
