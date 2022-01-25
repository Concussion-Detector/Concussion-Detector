import numpy as np
import matplotlib.pyplot as plt
from flask import Flask, jsonify

app = Flask(__name__)

@app.route("/")
def index():
    x = [1, 2, 3, 4, 5]
    y = [2, 4, 3, 7, 5]

    #plt.scatter(x, y)
    #plt.show()
    print("Running Python")
    return "Okay"

if __name__ == "__main__":
    app.run()