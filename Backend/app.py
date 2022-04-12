from sys import stdout
from process import webopencv
import cv2
import logging
from flask import Flask, render_template, Response, request, jsonify
from flask_socketio import SocketIO
from camera import Camera
from eye_tracking import GazeTracking
from utils import base64_to_pil_image, pil_image_to_base64
import os

gaze = GazeTracking()
app = Flask(__name__)
app.logger.addHandler(logging.StreamHandler(stdout))
app.config['DEBUG'] = True
socketio = SocketIO(app)
camera = Camera(webopencv())

@socketio.on('input image', namespace='/test')
def test_message(input):
    input = input.split(",")[1]
    camera.enqueue_input(input)
    #camera.enqueue_input(base64_to_pil_image(input))


@socketio.on('connect', namespace='/test')
def test_connect():
    app.logger.info("client connected")

xpupil = []
ypupil = []

# save coords in csv file
@app.route('/')
def index():
    return  render_template('index.html')

def face_detection():
    while True:
        _, frame = camera.get_frame()
        # Analyze frame
        gaze.refresh(frame)

        frame = gaze.draw()

        left_pupil = gaze.pupil_left_coords()
        right_pupil = gaze.pupil_right_coords()

        # print out the pupil coords
        cv2.putText(frame, "Left pupil:  " + str(left_pupil), (20, 70), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)
        cv2.putText(frame, "Right pupil: " + str(right_pupil), (20, 105), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)

        # ret, buffer = cv2.imencode('.jpg',frame)
        # frame = buffer.tobytes()
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n'+frame+ b'\r\n')
        
        
@app.route('/track')
def track():
    return Response(face_detection(), mimetype='multipart/x-mixed-replace; boundary=frame')

#app.run(debug=True)
socketio.run(app)