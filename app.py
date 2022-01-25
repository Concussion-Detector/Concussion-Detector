import numpy as np
import matplotlib.pyplot as plt
from flask import Flask, jsonify
import cv2
import numpy as np
import eyeMotion

app = Flask(__name__)

@app.route("/")
def index():
    print("Running Python")
    eyeMotion.main()
    return "OK"

if __name__ == "__main__":
    app.run()