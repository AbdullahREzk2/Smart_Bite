import torch
import numpy as np
import cv2
from tensorflow import keras

yolo_model = torch.hub.load('ultralytics/yolov5', 'yolov5s', pretrained=True, trust_repo=True)
classification_model = keras.models.load_model('best_modelPRE.h5')

with open('classes.txt', 'r') as f:
    class_names = [line.strip() for line in f.readlines()]

def process_image_yolov5(img_path, output_path='output_result.jpg'):
    img = cv2.imread(img_path)
    img_rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    
    results = yolo_model(img_path)
    boxes = results.xyxy[0].cpu().numpy()

    output_img = img_rgb.copy()
    predicted_labels = []

    for i, box in enumerate(boxes):
        x1, y1, x2, y2, conf, cls = box.astype(int)
        cropped = img_rgb[y1:y2, x1:x2]
        cropped_resized = cv2.resize(cropped, (160, 160))
        cropped_resized = cropped_resized / 255.0
        cropped_resized = np.expand_dims(cropped_resized, axis=0)

        pred = classification_model.predict(cropped_resized, verbose=0)
        pred_class = np.argmax(pred)
        pred_class_name = class_names[pred_class]
        


        predicted_labels.append(pred_class_name)

        cv2.rectangle(output_img, (x1, y1), (x2, y2), (0, 255, 0), 2)
        cv2.putText(output_img, pred_class_name, (x1, y1 - 20),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.9, (255, 255, 0), 2)

    cv2.imwrite(output_path, cv2.cvtColor(output_img, cv2.COLOR_RGB2BGR))
    return output_path, predicted_labels

