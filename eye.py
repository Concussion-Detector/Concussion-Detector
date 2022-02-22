import math
import numpy as np
import cv2

class Eye(object):
    """
    This class creates a new frame to isolate the eye and
    initiates the pupil detection.
    """

    # see 68_face landmarks for more info
    LEFT_EYE_POINTS = [36, 37, 38, 39, 40, 41]
    RIGHT_EYE_POINTS = [42, 43, 44, 45, 46, 47]

    def __init__(self, original_frame, landmarks, side, calibration):
        self.frame = None
        self.origin = None
        self.center = None
        self.pupil = None
        self.landmark_points = None
    
    @staticmethod
    def _middle_point(p1, p2):
        # returns the midpoint of 2 points
        # cast to int cos it is in the pixels (pixels cannot be halfs eg. 501px / 2 = 250.5)
        x = int((p1.x + p2.x) / 2)
        y = int((p1.y + p2.y) / 2)
        return (x, y)