from flask import Flask, render_template, Response
import cv2
from eye_tracking import GazeTracking

cap = cv2.VideoCapture(0)
gaze = GazeTracking()
app = Flask(__name__)

@app.route('/')

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

        # if left_pupil and right_pupil and record == "true":
        #     x = int((left_pupil[0] + right_pupil[0]) / 2)
        #     y = int((left_pupil[1] + right_pupil[1]) / 2)

        #     xpupil.append(x)
        #     ypupil.append(y)
            
        #     save_to_file(x, y, writeToFileCSV)
        

        #data = sock.ReadReceivedData() # read data

        # if data is not None:
        #     print(data)
        #     if data == "true" or data == "false":
        #         record = data
        #     else:
        #          tempData = data.split()
        #          option = tempData[0]
        #          uuid = tempData[1]

        #cv2.imshow("Frame",frame)

        ret, buffer = cv2.imencode('.jpg',frame)
        frame = buffer.tobytes()
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n'+frame+ b'\r\n')
        
@app.route('/track')

def track():
    return Response(face_detection(), mimetype='multipart/x-mixed-replace; boundary=frame')

app.run(debug=True)