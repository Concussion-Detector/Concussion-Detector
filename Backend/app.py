from flask import Flask, render_template, Response
import cv2
from eye_tracking import GazeTracking

cap = cv2.VideoCapture(0)
gaze = GazeTracking()
app = Flask(__name__)

xpupil = []
ypupil = []

@app.route('/')


# TODO
# save coords in csv file

def index():
    return  render_template('index.html')

def face_detection():
    while True:
        _, frame = cap.read()
        # Analyze frame
        gaze.refresh(frame)

        frame = gaze.draw()

        left_pupil = gaze.pupil_left_coords()
        right_pupil = gaze.pupil_right_coords()

        # print out the pupil coords
        cv2.putText(frame, "Left pupil:  " + str(left_pupil), (20, 70), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)
        cv2.putText(frame, "Right pupil: " + str(right_pupil), (20, 105), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)

        ret, buffer = cv2.imencode('.jpg',frame)
        frame = buffer.tobytes()
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n'+frame+ b'\r\n')
        
        
@app.route('/track')

def track():
    return Response(face_detection(), mimetype='multipart/x-mixed-replace; boundary=frame')

app.run(debug=True)