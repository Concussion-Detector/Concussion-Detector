import cv
import numpy as np
import dlib

cap = cv2.VideoCapture(0)

detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor("shape_68.dat")

# see 68_face landmarks for more info
LEFT_EYE_POINTS = [36, 37, 38, 39, 40, 41]
RIGHT_EYE_POINTS = [42, 43, 44, 45, 46, 47]


# returns the midpoint of 2 points
# cast to int cos it is in the pixels (pixels cannot be halfs eg. 501px / 2 = 250.5)
def midpoint(p1,p2):
    return int((p1.x + p2.x) / 2), int((p1.y + p2.y) /2)

def pupil_left_coords():
    if self.pupils_located:
        x = self.eye_left.origin[0] + self.eye_left.pupil.x
        y = self.eye_left.origin[1] + self.eye_left.pupil.y
        return (x, y)

def pupil_right_coords():
    if self.pupils_located:
        x = self.eye_right.origin[0] + self.eye_right.pupil.x
        y = self.eye_right.origin[1] + self.eye_right.pupil.y
        return (x, y)

while True:
    _, frame = cap.read()

    gray = cv2.cvtColor(frame,cv2.COLOR_BGR2GRAY)

    # an array of faces
    faces = detector(gray)

    for face in faces:
        x, y = face.left(), face.top()
        
        x1, y1 = face.right(), face.bottom()
        cv2.rectangle(frame, (x, y), (x1, y1), (0, 255, 0), 2)

        # find the landmarks on the gray frame, in the specific position (face)
        landmarks = predictor(gray,face)

        # LEFT EYE X,Y
        left_x = int((landmarks.part(36).x + landmarks.part(39).x) /2)
        left_y = int((landmarks.part(36).y + landmarks.part(39).y) /2)

        # RIGHT EYE X,Y
        right_x = int((landmarks.part(42).x + landmarks.part(45).x) /2)
        right_y = int((landmarks.part(42).y + landmarks.part(45).y) /2)


        left_point_l = (landmarks.part(36).x, landmarks.part(36).y)
        right_point_l = (landmarks.part(39).x, landmarks.part(39).y)

        left_point_r = (landmarks.part(42).x, landmarks.part(42).y)
        right_point_r = (landmarks.part(45).x, landmarks.part(45).y)

        cv2.circle(frame, (left_x,left_y), 3, (0,0,255),1)

        cv2.circle(frame, (right_x,right_y), 3, (0,0,255),1)



        # print out the pupil coords
        cv2.putText(frame, "Left pupil:  " + str(left_x) + " ," + str(left_y),
                    (90, 130), cv2.FONT_HERSHEY_DUPLEX, 0.9, (0,0,255), 1)

        cv2.putText(frame, "Right pupil:  " + str(right_x) + " ," + str(right_y),
                    (90, 100), cv2.FONT_HERSHEY_DUPLEX, 0.9, (0,0,255), 1)


    cv2.imshow("Frame",frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break
        


cap.release()
