from __future__ import division # True division
import os
import cv2
from cv2.data import haarcascades
import dlib
from .eye import Eye
from .calibration import Calibration

class GazeTracking(object):
    """
    This class tracks the user's gaze.
    It also provides pupils coords.
    """

    def __init__(self):
        self.frame = None
        self.eye_left = None
        self.eye_right = None
        self.calibration = Calibration()

        # _face_detector is used to detect faces
        self._detector = dlib.get_frontal_face_detector()
        cwd = os.path.abspath(os.path.dirname(__file__))
        model_path = os.path.abspath(os.path.join(cwd, "shape_predictor_68_face_landmarks.dat"))
        # _predictor is used to get facial landmarks of a given face
        self._predictor = dlib.shape_predictor(model_path)

    @property
    def pupils_located(self):
        """Check that the pupils have been located"""
        try:
            int(self.eye_left.pupil.x)
            int(self.eye_left.pupil.y)
            int(self.eye_right.pupil.x)
            int(self.eye_right.pupil.y)
            return True
        except Exception:
            return False
    
    def _analyze(self):
        """Detects the face and initialize Eye objects"""

        # Change the frame to grayscale for quicker computations
        frame = cv2.cvtColor(self.frame, cv2.COLOR_BGR2GRAY)
        faces = self._detector(frame)

        try:
            landmarks = self._predictor(frame, faces[0])
            self.eye_left = Eye(frame, landmarks, 0, self.calibration)
            self.eye_right = Eye(frame, landmarks, 1, self.calibration)

        except IndexError:
            self.eye_left = None
            self.eye_right = None

    def refresh(self, frame):
        """Refreshes the frame and analyzes it."""
        
        self.frame = frame
        self._analyze()

    def pupil_left_coords(self):
        """Returns the coordinates of the left pupil"""
        if self.pupils_located:
            x = self.eye_left.origin[0] + self.eye_left.pupil.x
            y = self.eye_left.origin[1] + self.eye_left.pupil.y
            return (x, y)

    def pupil_right_coords(self):
        """Returns the coordinates of the right pupil"""
        if self.pupils_located:
            x = self.eye_right.origin[0] + self.eye_right.pupil.x
            y = self.eye_right.origin[1] + self.eye_right.pupil.y
            return (x, y)
    
    def draw(self):
        """Returns the main frame draws coords of the pupils and 
        rectangle on the detected face to help with proper face alignment."""
        frame = self.frame.copy()

        face_detector = cv2.CascadeClassifier(haarcascades + "haarcascade_frontalface_default.xml")

        faces = face_detector.detectMultiScale(frame, 1.1, 4)
        #faces = self._detector(frame)

        # Shape of the frame
        height, width, channels = frame.shape

        # Frame center
        height_center = height//2
        width_center = width//2

        # Center of the rectangle
        upper_left = (width // 4, height // 4)
        bottom_right = (width * 3 // 4, height * 3 // 4)

        for (x, y, w, h) in faces:
            centre_x = x + w//2
            centre_y = y + y//2

            # Center of a screen
            #cv2.putText(frame, ".", (int(width_center), int(height_center)), cv2.FONT_HERSHEY_DUPLEX, 0.9, (255, 255, 0), 1)
            cv2.circle(frame, (int(width_center), int(height_center)), 2, (255, 255, 0), 1)

            cv2.rectangle(frame, upper_left, bottom_right, (0, 255, 0), thickness=1)

            if centre_x in range(width_center-5, width_center+5,1) or centre_y in range(height_center-5,height_center+5,1):
                print("Centered")
                cv2.putText(frame, "Face Aligned.", (20, 30), cv2.FONT_HERSHEY_DUPLEX, 0.9, (147, 58, 31), 1)
                cv2.rectangle(frame, (x, y), (x+w, y+h), (0, 255, 0), 2)

        if self.pupils_located:
            color = (255, 0, 0)
            x_left, y_left = self.pupil_left_coords()
            x_right, y_right = self.pupil_right_coords()

            # Draw Circles around the eyes
            cv2.circle(frame, (x_left, y_left), 4, (0, 0, 255), 1)
            cv2.circle(frame, (x_right, y_right), 4, (0, 0, 255), 1)

        return frame
    