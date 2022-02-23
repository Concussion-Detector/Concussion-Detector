import cv2
from eye_tracking import GazeTracking
import UdpComms as U
import time
import logging

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

gaze = GazeTracking()
cap = cv2.VideoCapture(0)

while True:
    _, frame = cap.read()

    # Analyze frame
    gaze.refresh(frame)

    frame = gaze.draw()
    # text = ""

    # if gaze.is_center():
    #     text = "Looking center"
    # elif gaze.is_right():
    #     text = "Looking right"
    # elif gaze.is_left():
    #     text = "Looking left"
    
    #cv2.putText(frame, text, (90, 60), cv2.FONT_HERSHEY_DUPLEX, 1.6, (147, 58, 31), 2)


    left_pupil = gaze.pupil_left_coords()
    right_pupil = gaze.pupil_right_coords()
    # print out the pupil coords
    cv2.putText(frame, "Left pupil:  " + str(left_pupil), (90, 130), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)
    cv2.putText(frame, "Right pupil: " + str(right_pupil), (90, 165), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)

    sock.SendData(left_pupil) # Send this string to other application
    sock.SendData(right_pupil)
    
    data = sock.ReadReceivedData() # read data

    cv2.imshow("Frame",frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break
        
cap.release()
cv2.destroyAllWindows()
