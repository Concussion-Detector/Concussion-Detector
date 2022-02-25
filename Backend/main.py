import cv2
from eye_tracking import GazeTracking
import UdpComms as U
import time
import logging
import matplotlib.pyplot as plt
from my_database import Database

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

db = Database()
gaze = GazeTracking()
cap = cv2.VideoCapture(0)

xpupil = []
ypupil = []

data = ""

writeToFileCSV = open("./files/eye-coordinatesCSV.csv", "w")

def save_to_file(x, y, fileName):
     fileName.write(str(x) + ", " + str(y) + "\n")

while True:
    _, frame = cap.read()

    # Analyze frame
    gaze.refresh(frame)

    frame = gaze.draw()

    left_pupil = gaze.pupil_left_coords()
    right_pupil = gaze.pupil_right_coords()

    # print out the pupil coords
    cv2.putText(frame, "Left pupil:  " + str(left_pupil), (90, 130), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)
    cv2.putText(frame, "Right pupil: " + str(right_pupil), (90, 165), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)

    

    if left_pupil and right_pupil: #and data == "false":
        x = int((left_pupil[0] + right_pupil[0]) / 2)
        y = int((left_pupil[1] + right_pupil[1]) / 2)

        xpupil.append(x)
        ypupil.append(y)

        save_to_file(x, y, writeToFileCSV)
    

    tempData = sock.ReadReceivedData() # read data

    if tempData == "true" or tempData == "false":
        data = tempData

    if tempData == "baseline":
        print("Baseline Test")
    elif tempData == "concdata":
        print("Concussion Test")

    cv2.imshow("Frame",frame)

    if cv2.waitKey(1) & 0xFF == ord('q'): #or data == "true":
        writeToFileCSV.close()
        break
        
cap.release()
cv2.destroyAllWindows()

plt.scatter(xpupil, ypupil, zorder=2)
plt.show()

f = open('./files/eye-coordinatesCSV.csv')

coords = f.read()

test = {
    "user_id": "001",
    "test_data": coords
}

db.SaveToDatabase('001', coords)
