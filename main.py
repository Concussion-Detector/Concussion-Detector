import cv2
from eye_tracking import GazeTracking
import UdpComms as U
import time
import logging
import matplotlib.pyplot as plt

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

gaze = GazeTracking()
cap = cv2.VideoCapture(0)

xpupil = []
ypupil = []

data = ""

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
    #print(type(left_pupil))
    # print out the pupil coords
    cv2.putText(frame, "Left pupil:  " + str(left_pupil), (90, 130), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)
    cv2.putText(frame, "Right pupil: " + str(right_pupil), (90, 165), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)

    

    if left_pupil and right_pupil and data == "false":
        x = int((left_pupil[0] + right_pupil[0]) / 2)
        y = int((left_pupil[1] + right_pupil[1]) / 2)

        xpupil.append(x)
        ypupil.append(y)
    # if left_pupil:
    #     left_x = left_pupil[0]
    #     left_y = left_pupil[1]
    #     sock.SendData("l,"+str(left_x)+","+str(left_y)) # Send this string to other application

    #     #print(left_x,left_y)

    # if right_pupil:
    #     right_x = right_pupil[0]
    #     right_y = right_pupil[1]
    #     sock.SendData("r,"+str(right_x)+","+str(right_y))
    

    tempData = sock.ReadReceivedData() # read data
    if tempData == "true" or tempData == "false":
        data = tempData

    cv2.imshow("Frame",frame)

    if cv2.waitKey(1) & 0xFF == ord('q') or data == "true":
        break
        
cap.release()
cv2.destroyAllWindows()

plt.scatter(xpupil, ypupil, zorder=2)
plt.show()
