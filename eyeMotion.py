import cv2
import numpy as np
import matplotlib.pyplot as plt

# Face and Eyes Classifiers
face_cascade = cv2.CascadeClassifier('haarcascade_frontalface_default.xml')
eye_cascade = cv2.CascadeClassifier('haarcascade_eye.xml')
#Blob detection
detector_params = cv2.SimpleBlobDetector_Params()
detector_params.filterByArea = True
detector_params.maxArea = 1500
detector = cv2.SimpleBlobDetector_create(detector_params)


def detect_faces(img, cascade):
    gray_frame = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    coords = cascade.detectMultiScale(gray_frame, 1.3, 5)
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
    return frame


def detect_eyes(img, cascade):
    gray_frame = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    eyes = cascade.detectMultiScale(gray_frame, 1.3, 5)  # detect eyes
    width = np.size(img, 1)  # get face frame width
    height = np.size(img, 0)  # get face frame height
    left_eye = None
    right_eye = None
    for (x, y, w, h) in eyes:
        if y > height / 2: # skip if eye is at the bottom
            pass
        eyecenter = x + w / 2  # get the eye center
        if eyecenter < width * 0.5:
            left_eye = img[y:y + h, x:x + w]
        else:
            right_eye = img[y:y + h, x:x + w]
        #if left_eye is None:
            #print("left eye not detected")
        #if right_eye is None:
            #print("right eye not detected")

    return left_eye, right_eye


# Cut out unnecessary space to increase a precision
def cut_eyebrows(img):
    height, width = img.shape[:2]
    eyebrow_h = int(height / 4)
    img = img[eyebrow_h:height, 0:width]  # cut eyebrows out (15 px)

    return img


def blob_process(img, threshold, detector):
    gray_frame = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    _, img = cv2.threshold(gray_frame, threshold, 255, cv2.THRESH_BINARY)
    # Erosions and dialtions to reduce a noise
    img = cv2.erode(img, None, iterations=2)
    img = cv2.dilate(img, None, iterations=4)
    img = cv2.medianBlur(img, 5)
    keypoints = detector.detect(img)
    #print(keypoints)
    return keypoints


def nothing(x):
    pass


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
    cv2.namedWindow('image')
    cv2.createTrackbar('threshold', 'image', 0, 255, nothing)
    i = 0
    while True:
        _, frame = cap.read()
        face_frame = detect_faces(frame, face_cascade)
        if face_frame is not None:
            eyes = detect_eyes(face_frame, eye_cascade)
            for eye in eyes:     
                #if i == 2:
                    #i = 0 
                if eye is not None:
                    #print("eye " + (i+1))
                    threshold = r = cv2.getTrackbarPos('threshold', 'image')
                    eye = cut_eyebrows(eye)
                    #print('an eyes {eyes}'.format(eyes=eyes))
                    keypoints = blob_process(eye, threshold, detector)
                    #eye = cv2.drawKeypoints(eye, keypoints, eye, (0, 0, 255), cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS)
                    eye = draw_custom_KeyPoints(eye,keypoints,(0,255,0),1)
                    kp = cv2.KeyPoint_convert(keypoints) # converts keypoints to ndarray 
                    if keypoints: #if the keypoint is null it skips it
                        x.append(kp.flat[0]) #adds the x coords
                        y.append(kp.flat[1]) #1d iterator over the array
                        #x.append(keypoints.pt[0])
                        #y.append(keypoints.pt[1])
                        print('x: {x}, y: {y}'.format( x=kp.flat[0], y=kp.flat[1]))
                        # Drawing the points where an eye is currently look at
                       
                            #cv2.circle(eye,(int(kp.flat[0]), int(kp.flat[1])), 2,1)
        
        cv2.imshow('image', frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    cap.release()
    #cv2.destroyAllWindows()
    i = 0
    plt.imshow(eye,zorder=1)
    plt.scatter(x, y,zorder =2) #plot the points
    plt.show()


if __name__ == "__main__":
    main()