from fastapi import FastAPI
from fastapi.responses import PlainTextResponse
from pydantic import BaseModel
import numpy as np
import joblib

app = FastAPI()

nutrients = [
    "Protien", "Saturated Fat", "Carbs", "Suger",
    "Fat", "Cholecterol", "Fiber", "Sodium"
]

models = {}
scalers = {}
polys = {}

for nutrient in nutrients:
    models[nutrient] = joblib.load(f"models/{nutrient}_model.pkl")
    scalers[nutrient] = joblib.load(f"models/scaler_{nutrient}.pkl")
    polys[nutrient] = joblib.load(f"models/poly_{nutrient}.pkl")

class UserInput(BaseModel):
    RIAGENDR: int       
    RIDAGEYR: float     
    BMXWT: float        
    BMXHT: float        
    BMXBMI: float       
    BPQ020: int         
    BPQ050A: int        
    DIQ010: int         
    obesity: int 
    DIQ050: int         
    MCQ300A: int        
    activity_level: int 
    BMR: float
    

    

@app.post("/predict_all", response_class=PlainTextResponse)
def predict_all(data: UserInput):
    features = np.array([[data.RIAGENDR, data.RIDAGEYR, data.BMXWT, data.BMXHT,
                      data.BMXBMI, data.BPQ020, data.BPQ050A, data.DIQ010,
                      data.DIQ050, data.MCQ300A, data.activity_level, data.BMR]])

    
    results = {}

    for nutrient in nutrients:
        poly = polys[nutrient].transform(features)
        scaled = scalers[nutrient].transform(poly)
        pred_log = models[nutrient].predict(scaled)
        pred = np.expm1(pred_log)
        results[nutrient] = float(round(pred[0], 2))

    result_string = '\n'.join([f"{k}: {v}" for k, v in results.items()])
    return result_string
