import numpy as np
import matplotlib.pyplot as plt
from flask import Flask, jsonify
import cv2
import numpy as np

app = Flask(__name__)

@app.route("/")
def index():
    x = [1, 2, 3, 4, 5]
    y = [2, 4, 3, 7, 5]

    cap = cv2.VideoCapture(0)

    while True:
        _, img = cap.read()

        cv2.imshow('img', img)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
        
    cap.release()
    cv2.destroyAllWindows()

    #plt.scatter(x, y)
    #plt.show()
    print("Running Python")
    return "OK"

if __name__ == "__main__":
  app.run()