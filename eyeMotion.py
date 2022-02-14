import cv2
import numpy as np
import matplotlib.pyplot as plt
from cv2.data import haarcascades
import UdpComms as U
import time
import logging

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

logger = logging.getLogger(__name__)

# Face and Eyes Classifiers
face_detector = cv2.CascadeClassifier(haarcascades + "haarcascade_frontalface_default.xml")
eye_detector = cv2.CascadeClassifier(haarcascades + "haarcascade_eye.xml")

#Blob detection
detector_params = cv2.SimpleBlobDetector_Params()
detector_params.filterByArea = True
detector_params.maxArea = 1500
detector = cv2.SimpleBlobDetector_create(detector_params)

def detect_faces(img):
    #gray_frame = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    coords = face_detector.detectMultiScale(img, 1.3, 5)
    if len(coords) > 1:
        biggest = (0, 0, 0, 0)
        for i in coords:
            if i[3] > biggest[3]:
                biggest = i
        biggest = np.array([i], np.int32)
    elif len(coords) == 1:
        biggest = coords
    else:
        return None
    for (x, y, w, h) in biggest:
        frame = img[y:y + h, x:x + w]
        # to draw a rectangle
        cv2.rectangle(img,(x,y),(x+w,y+h),(255,255,0),2)
    return frame

def detect_eyes(img):
    #gray_frame = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    eyes = eye_detector.detectMultiScale(img, 1.3, 5)  # detect eyes
    width = np.size(img, 1)  # get face frame width
    height = np.size(img, 0)  # get face frame height
    left_eye = None
    right_eye = None

    if eyes is None or len(eyes) == 0:
        return left_eye, right_eye
    for (x, y, w, h) in eyes:
        if y > height / 2: # skip if eye is at the bottom
            pass
        eyecenter = int(float(x) + (float(w) / float(2)))  # get the eye center
        if eyecenter < width * 0.5:
            left_eye = img[y:y + h, x:x + w]
        else:
            right_eye = img[y:y + h, x:x + w]
    #print('left: height {h} width {w}'.format(h=np.size(left_eye, 0), w= np.size(left_eye, 1)))
    #print('right: height {h} width {w}'.format(h=right_eye.shape[0], w= right_eye.shape[1]))
    return left_eye, right_eye

# Cut out unnecessary space to increase a precision
def cut_eyebrows(img):
    height, width = img.shape[:2]
    eyebrow_h = int(height / 4)
    img = img[eyebrow_h:height, 0:width]  # cut eyebrows out (15 px)

    return img

def blob_process(img, threshold, detector, prev_area):
    #gray_frame = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    _, img = cv2.threshold(img, threshold, 255, cv2.THRESH_BINARY)
    # Erosions and dialtions to reduce a noise
    img = cv2.erode(img, None, iterations=2)
    img = cv2.dilate(img, None, iterations=4)
    img = cv2.medianBlur(img, 5)
    keypoints = detector.detect(img)
    if keypoints and len(keypoints) > 1:
        tmp = 1000
        for kp in keypoints: # filtering odd blobs
            if abs(kp.size - prev_area) < tmp:
                ans = kp
                tmp = abs(kp.size - prev_area)
        
        keypoints = (ans,)
    #print(keypoints)
    return keypoints


def nothing(x):
    pass


def draw(source, keypoints, dest=None):
        try:
            if dest is None:
                dest = source
            return cv2.drawKeypoints(
                source,
                keypoints,
                dest,
                (0, 0, 255),
                cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS,
            )
        except cv2.error as e:
            raise CV2Error(str(e))

# Custom draw keyPoints method
def draw_custom_KeyPoints(img,keypoints, col, thickness):
    for current_kp in keypoints:
        x=np.int(current_kp.pt[0])
        y=np.int(current_kp.pt[1])
        size = np.int(current_kp.size/2) # used as a radius of a circle
        cv2.circle(img,(x,y),size,col,thickness=thickness, lineType=8, shift=0)
    #plt.imshow(img)
    return img

# Coordinates of an eye
x = []
y = []

def main():
    cap = cv2.VideoCapture(0)
    

    if not cap.isOpened():
        raise IOError("Cannot open a webcam.")
        
    cv2.namedWindow('image')
    cv2.createTrackbar('threshold', 'image', 0, 255, nothing)
    i = 0
    left_eye_kp = None
    right_eye_kp = None
    previous_left_kp = None
    previous_right_kp = None
    while True:
        _, frame = cap.read()
        face_frame = detect_faces(frame)

        if face_frame is not None:
            gray_frame = cv2.cvtColor(face_frame, cv2.COLOR_BGR2GRAY)
        # #if face_frame is not None:
        
            left_eye,right_eye = detect_eyes(gray_frame)

            threshold = r = cv2.getTrackbarPos('threshold', 'image')
            
            if left_eye is not None:
                #print("Left eye detected")
                l_eye = cut_eyebrows(left_eye)
                left_eye_kp = blob_process(l_eye, threshold, detector, 1)

                kp = left_eye_kp or previous_left_kp
                #p = cv2.KeyPoint_convert(kp)

            
                #left_eye = draw(left_eye, 9kp, gray_frame)
                p = cv2.KeyPoint_convert(left_eye_kp)
                if left_eye_kp:
                    x = str(p.flat[0])
                    y = str(p.flat[1])
                    coords = 'l' + ',' + x + ',' + y
                    print('left eye {xy}'.format(xy=coords))

                    sock.SendData(coords) # Send this string to other application

                    data = sock.ReadReceivedData() # read data

                left_eye = draw(left_eye, kp,frame)
                previous_left_kp = kp
            #else:
                #print("left eye not detected")
            
            if right_eye is not None:
                r_eye = cut_eyebrows(right_eye)
                right_eye_kp = blob_process(right_eye, threshold, detector, 1)

                kp = right_eye_kp or previous_right_kp
                p = cv2.KeyPoint_convert(kp)
                if right_eye_kp:
                    x = str(p.flat[0])
                    y = str(p.flat[1])
                    coords = 'r' + ',' + x + ',' + y
                    print('right eye {xy}'.format(xy=coords))

                    sock.SendData(coords) # Send this string to other application

                    data = sock.ReadReceivedData() # read data

                right_eye = draw(right_eye, kp,frame)
                previous_right_kp = kp








            # for eye in eyes:     
            #     #if i == 2:
            #         #i = 0 
            #     if eye is not None:
            #         #print("eye " + (i+1))
            #         threshold = r = cv2.getTrackbarPos('threshold', 'image')
            #         eye = cut_eyebrows(eye)
            #         #print('an eyes {eyes}'.format(eyes=eyes))
            #         keypoints = blob_process(eye, threshold, detector,1)
            #         #eye = cv2.drawKeypoints(eye, keypoints, eye, (0, 0, 255), cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS)
            #         eye = draw_custom_KeyPoints(eye,keypoints,(0,255,0),1)
            #         kp = cv2.KeyPoint_convert(keypoints) # converts keypoints to ndarray 
            #         if keypoints: #if the keypoint is null it skips it
            #             x.append(kp.flat[0]) #adds the x coords
            #             y.append(kp.flat[1]) #1d iterator over the array
            #             #x.append(keypoints.pt[0])
            #             #y.append(keypoints.pt[1])
            #             print('x: {x}, y: {y}'.format( x=kp.flat[0], y=kp.flat[1]))
                        # Drawing the points where an eye is currently look at
                       
                            #cv2.circle(eye,(int(kp.flat[0]), int(kp.flat[1])), 2,1)
        
        

        
        cv2.imshow('image', frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
        
    #print('height {h} width {w}'.format(h=height, w= width))
    cap.release()
    #cv2.destroyAllWindows()
    i = 0
    #plt.imshow(eye,zorder=1)
    #plt.scatter(x, y,zorder =2) #plot the points
    #plt.show()


if __name__ == "__main__":
    main()