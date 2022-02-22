from __future__ import division
import os
import cv2
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
        # _predictor is used to get facial landmarks of a given face
        self._predictor = dlib.shape_predictor("shape_68.dat")