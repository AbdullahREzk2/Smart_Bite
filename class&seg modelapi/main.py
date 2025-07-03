from fastapi import FastAPI, UploadFile, File
from fastapi.responses import FileResponse, JSONResponse
import shutil
import os
from model import process_image_yolov5

app = FastAPI()

UPLOAD_DIR = "uploads"
RESULT_DIR = "results"

os.makedirs(UPLOAD_DIR, exist_ok=True)
os.makedirs(RESULT_DIR, exist_ok=True)

@app.get("/")
def root():
    return {"message": "Welcome to the AI food detection API!"}

@app.post("/predict")
async def predict(file: UploadFile = File(...)):
    file_path = os.path.join(UPLOAD_DIR, file.filename)
    with open(file_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)
        print(type(File)) 

    output_path = os.path.join(RESULT_DIR, f"result_{file.filename}")
    result_img_path, predictions = process_image_yolov5(file_path, output_path)

    return {
        "predicted_labels": predictions,
        "output_image": result_img_path
    }

@app.get("/download/{filename}")
def download_result(filename: str):
    file_path = os.path.join(RESULT_DIR, filename)
    if os.path.exists(file_path):
        return FileResponse(file_path, media_type="image/jpeg", filename=filename)
    return JSONResponse(status_code=404, content={"message": "File not found"})
