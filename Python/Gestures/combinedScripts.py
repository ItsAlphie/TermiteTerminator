#!/usr/bin/env python
# -*- coding: utf-8 -*-

# Mutual imports
import cv2 as cv2
import numpy as np

# Imports gesture recognition
import csv
import copy
import argparse
import itertools
import math
import socket
import re
from collections import Counter
from collections import deque

import mediapipe as mp

from utils import CvFpsCalc
from model import KeyPointClassifier
from model import PointHistoryClassifier

# Imports for fiducial tracking
from imutils.video import VideoStream
import argparse
import imutils
import time
import sys
import socket
import time

shared_video_stream = VideoStream(src=1).start()
time.sleep(1)

# Requirements for TCP communication to Unity
ip, port = "127.0.0.1", 11000

def sendData(data):	
	sock = socket.socket(socket.AF_INET,
                     socket.SOCK_DGRAM)
	sock.sendto(data.encode(), (ip, port))
     
############################################################################################
#################################### Gesture Recognition ###################################
############################################################################################

def get_args():
    parser = argparse.ArgumentParser()

    parser.add_argument("--device", type=int, default=0)
    parser.add_argument("--width", help='cap width', type=int, default=960)
    parser.add_argument("--height", help='cap height', type=int, default=540)

    parser.add_argument('--use_static_image_mode', action='store_true')
    parser.add_argument("--min_detection_confidence",
                        help='min_detection_confidence',
                        type=float,
                        default=0.7)
    parser.add_argument("--min_tracking_confidence",
                        help='min_tracking_confidence',
                        type=int,
                        default=0.5)

    args = parser.parse_args()

    return args

def select_mode(key, mode):
    number = -1
    if 48 <= key <= 57:  # 0 ~ 9
        number = key - 48
    if key == 110:  # n
        mode = 0
    if key == 107:  # k
        mode = 1
    if key == 104:  # h
        mode = 2
    return number, mode


def calc_bounding_rect(image, landmarks):
    image_width, image_height = image.shape[1], image.shape[0]

    landmark_array = np.empty((0, 2), int)

    for _, landmark in enumerate(landmarks.landmark):
        landmark_x = min(int(landmark.x * image_width), image_width - 1)
        landmark_y = min(int(landmark.y * image_height), image_height - 1)

        landmark_point = [np.array((landmark_x, landmark_y))]

        landmark_array = np.append(landmark_array, landmark_point, axis=0)

    x, y, w, h = cv2.boundingRect(landmark_array)

    return [x, y, x + w, y + h]


def calc_landmark_list(image, landmarks):
    image_width, image_height = image.shape[1], image.shape[0]

    landmark_point = []

    # Keypoint
    for _, landmark in enumerate(landmarks.landmark):
        landmark_x = min(int(landmark.x * image_width), image_width - 1)
        landmark_y = min(int(landmark.y * image_height), image_height - 1)
        # landmark_z = landmark.z

        landmark_point.append([landmark_x, landmark_y])

    return landmark_point


def pre_process_landmark(landmark_list):
    temp_landmark_list = copy.deepcopy(landmark_list)

    # Convert to relative coordinates
    base_x, base_y = 0, 0
    for index, landmark_point in enumerate(temp_landmark_list):
        if index == 0:
            base_x, base_y = landmark_point[0], landmark_point[1]

        temp_landmark_list[index][0] = temp_landmark_list[index][0] - base_x
        temp_landmark_list[index][1] = temp_landmark_list[index][1] - base_y

    # Convert to a one-dimensional list
    temp_landmark_list = list(
        itertools.chain.from_iterable(temp_landmark_list))

    # Normalization
    max_value = max(list(map(abs, temp_landmark_list)))

    def normalize_(n):
        return n / max_value

    temp_landmark_list = list(map(normalize_, temp_landmark_list))

    return temp_landmark_list


def pre_process_point_history(image, point_history):
    image_width, image_height = image.shape[1], image.shape[0]

    temp_point_history = copy.deepcopy(point_history)

    # Convert to relative coordinates
    base_x, base_y = 0, 0
    for index, point in enumerate(temp_point_history):
        if index == 0:
            base_x, base_y = point[0], point[1]

        temp_point_history[index][0] = (temp_point_history[index][0] -
                                        base_x) / image_width
        temp_point_history[index][1] = (temp_point_history[index][1] -
                                        base_y) / image_height

    # Convert to a one-dimensional list
    temp_point_history = list(
        itertools.chain.from_iterable(temp_point_history))

    return temp_point_history


def logging_csv(number, mode, landmark_list, point_history_list):
    if mode == 0:
        pass
    if mode == 1 and (0 <= number <= 9):
        csv_path = 'model/keypoint_classifier/keypoint.csv'
        with open(csv_path, 'a', newline="") as f:
            writer = csv.writer(f)
            writer.writerow([number, *landmark_list])
    if mode == 2 and (0 <= number <= 9):
        csv_path = 'model/point_history_classifier/point_history.csv'
        with open(csv_path, 'a', newline="") as f:
            writer = csv.writer(f)
            writer.writerow([number, *point_history_list])
    return


def draw_landmarks(image, landmark_point):
    if len(landmark_point) > 0:
        # Thumb
        cv2.line(image, tuple(landmark_point[2]), tuple(landmark_point[3]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[2]), tuple(landmark_point[3]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[3]), tuple(landmark_point[4]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[3]), tuple(landmark_point[4]),
                (255, 255, 255), 2)

        # Index finger
        cv2.line(image, tuple(landmark_point[5]), tuple(landmark_point[6]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[5]), tuple(landmark_point[6]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[6]), tuple(landmark_point[7]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[6]), tuple(landmark_point[7]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[7]), tuple(landmark_point[8]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[7]), tuple(landmark_point[8]),
                (255, 255, 255), 2)

        # Middle finger
        cv2.line(image, tuple(landmark_point[9]), tuple(landmark_point[10]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[9]), tuple(landmark_point[10]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[10]), tuple(landmark_point[11]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[10]), tuple(landmark_point[11]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[11]), tuple(landmark_point[12]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[11]), tuple(landmark_point[12]),
                (255, 255, 255), 2)

        # Ring finger
        cv2.line(image, tuple(landmark_point[13]), tuple(landmark_point[14]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[13]), tuple(landmark_point[14]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[14]), tuple(landmark_point[15]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[14]), tuple(landmark_point[15]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[15]), tuple(landmark_point[16]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[15]), tuple(landmark_point[16]),
                (255, 255, 255), 2)

        # Little finger
        cv2.line(image, tuple(landmark_point[17]), tuple(landmark_point[18]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[17]), tuple(landmark_point[18]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[18]), tuple(landmark_point[19]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[18]), tuple(landmark_point[19]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[19]), tuple(landmark_point[20]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[19]), tuple(landmark_point[20]),
                (255, 255, 255), 2)

        # Palm
        cv2.line(image, tuple(landmark_point[0]), tuple(landmark_point[1]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[0]), tuple(landmark_point[1]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[1]), tuple(landmark_point[2]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[1]), tuple(landmark_point[2]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[2]), tuple(landmark_point[5]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[2]), tuple(landmark_point[5]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[5]), tuple(landmark_point[9]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[5]), tuple(landmark_point[9]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[9]), tuple(landmark_point[13]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[9]), tuple(landmark_point[13]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[13]), tuple(landmark_point[17]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[13]), tuple(landmark_point[17]),
                (255, 255, 255), 2)
        cv2.line(image, tuple(landmark_point[17]), tuple(landmark_point[0]),
                (0, 0, 0), 6)
        cv2.line(image, tuple(landmark_point[17]), tuple(landmark_point[0]),
                (255, 255, 255), 2)

    # Key Points
    for index, landmark in enumerate(landmark_point):
        if index == 0:  # 手首1
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 1:  # 手首2
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 2:  # 親指：付け根
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 3:  # 親指：第1関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 4:  # 親指：指先
            cv2.circle(image, (landmark[0], landmark[1]), 8, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 8, (0, 0, 0), 1)
        if index == 5:  # 人差指：付け根
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 6:  # 人差指：第2関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 7:  # 人差指：第1関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 8:  # 人差指：指先
            cv2.circle(image, (landmark[0], landmark[1]), 8, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 8, (0, 0, 0), 1)
        if index == 9:  # 中指：付け根
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 10:  # 中指：第2関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 11:  # 中指：第1関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 12:  # 中指：指先
            cv2.circle(image, (landmark[0], landmark[1]), 8, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 8, (0, 0, 0), 1)
        if index == 13:  # 薬指：付け根
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 14:  # 薬指：第2関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 15:  # 薬指：第1関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 16:  # 薬指：指先
            cv2.circle(image, (landmark[0], landmark[1]), 8, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 8, (0, 0, 0), 1)
        if index == 17:  # 小指：付け根
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 18:  # 小指：第2関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 19:  # 小指：第1関節
            cv2.circle(image, (landmark[0], landmark[1]), 5, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 5, (0, 0, 0), 1)
        if index == 20:  # 小指：指先
            cv2.circle(image, (landmark[0], landmark[1]), 8, (255, 255, 255),
                      -1)
            cv2.circle(image, (landmark[0], landmark[1]), 8, (0, 0, 0), 1)

    return image


def draw_bounding_rect(use_brect, image, brect):
    if use_brect:
        # Outer rectangle
        cv2.rectangle(image, (brect[0], brect[1]), (brect[2], brect[3]),
                     (0, 0, 0), 1)

    return image


def draw_info_text(image, brect, handedness, hand_sign_text,
                   finger_gesture_text):
    cv2.rectangle(image, (brect[0], brect[1]), (brect[2], brect[1] - 22),
                 (0, 0, 0), -1)

    info_text = handedness.classification[0].label[0:]
    if hand_sign_text != "":
        info_text = info_text + ':' + hand_sign_text
    cv2.putText(image, info_text, (brect[0] + 5, brect[1] - 4),
               cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 1, cv2.LINE_AA)

    if finger_gesture_text != "":
        cv2.putText(image, "Finger Gesture:" + finger_gesture_text, (10, 60),
                   cv2.FONT_HERSHEY_SIMPLEX, 1.0, (0, 0, 0), 4, cv2.LINE_AA)
        cv2.putText(image, "Finger Gesture:" + finger_gesture_text, (10, 60),
                   cv2.FONT_HERSHEY_SIMPLEX, 1.0, (255, 255, 255), 2,
                   cv2.LINE_AA)

    return image


def draw_point_history(image, point_history):
    for index, point in enumerate(point_history):
        if point[0] != 0 and point[1] != 0:
            cv2.circle(image, (point[0], point[1]), 1 + int(index / 2),
                      (152, 251, 152), 2)

    return image


def draw_info(image, fps):
    cv2.putText(image, "FPS:" + str(fps), (10, 30), cv2.FONT_HERSHEY_SIMPLEX,
               1.0, (0, 0, 0), 4, cv2.LINE_AA)
    cv2.putText(image, "FPS:" + str(fps), (10, 30), cv2.FONT_HERSHEY_SIMPLEX,
               1.0, (255, 255, 255), 2, cv2.LINE_AA)

    mode_string = ['Logging Key Point', 'Logging Point History']
    #if 1 <= mode <= 2:
    #    cv2.putText(image, "MODE:" + mode_string[mode - 1], (10, 90),
    #               cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 1,
    #               cv2.LINE_AA)
    #    if 0 <= number <= 9:
    #        cv2.putText(image, "NUM:" + str(number), (10, 110),
    #                   cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 1,
    #                   cv2.LINE_AA)
    return image

def CheckTriangle(thumbL, indexL, thumbR, indexR, indexRootR, hand_sign_idR, hand_sign_idL):
    # Empty checks
    if (thumbL == []) or ( indexL == []) or ( thumbR == []) or ( indexR == []) or ( indexRootR == []):
        return False
    
    proximity = getDistance(indexRootR, indexR)
    distanceIndex = getDistance(indexR, indexL)
    distanceThumb = getDistance(thumbR, thumbL)

    if (distanceIndex <= proximity) & (distanceThumb <= proximity): # & (hand_sign_idL == 4) & (hand_sign_idR == 4):
        print("Triangle Detected")
        return True
    else:
        return False

def getDistance(point1, point2):
    distance = math.sqrt((point2[0] - point1[0])**2 + (point2[1] - point1[1])**2)
    return distance

def getCenter(point1, point2, point3, point4):
    X = (point1[0] + point2[0] + point3[0] + point4[0]) / 4
    Y = (point1[1] + point2[1] + point3[1] + point4[1]) / 4
    return X, Y

# Argument parsing #################################################################
args = get_args()

cap_device = args.device
cap_width = args.width
cap_height = args.height

use_static_image_mode = args.use_static_image_mode
min_detection_confidence = args.min_detection_confidence
min_tracking_confidence = args.min_tracking_confidence

use_brect = True

# Camera preparation ###############################################################
cap = VideoStream(src=1).start()
#cap.set(cv2.CAP_PROP_FRAME_WIDTH, cap_width)
#cap.set(cv2.CAP_PROP_FRAME_HEIGHT, cap_height)

# Model load #############################################################
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(
    static_image_mode=use_static_image_mode,
    max_num_hands=10,
    min_detection_confidence=min_detection_confidence,
    min_tracking_confidence=min_tracking_confidence,
)

keypoint_classifier = KeyPointClassifier()

point_history_classifier = PointHistoryClassifier()

# Read labels ###########################################################
with open('model/keypoint_classifier/keypoint_classifier_label.csv',
            encoding='utf-8-sig') as f:
    keypoint_classifier_labels = csv.reader(f)
    keypoint_classifier_labels = [
        row[0] for row in keypoint_classifier_labels
    ]
with open(
        'model/point_history_classifier/point_history_classifier_label.csv',
        encoding='utf-8-sig') as f:
    point_history_classifier_labels = csv.reader(f)
    point_history_classifier_labels = [
        row[0] for row in point_history_classifier_labels
    ]

# FPS Measurement ########################################################
cvFpsCalc = CvFpsCalc(buffer_len=10)

# Coordinate history #################################################################
history_length = 16
point_history = deque(maxlen=history_length)

# Finger gesture history ################################################
finger_gesture_history = deque(maxlen=history_length)

def GestureMain(frame):
    fps = cvFpsCalc.get()
    image = frame
    debug_image = copy.deepcopy(frame)

    # Detection implementation #############################################################
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

    image.flags.writeable = False
    results = hands.process(image)
    image.flags.writeable = True

    #  ####################################################################
    if results.multi_hand_landmarks is not None:
        palmR = []
        palmL = []
        indexR = []
        indexRootR = []
        indexL = []
        thumbR = []
        thumbL = []
        hand_sign_idR = 0
        hand_sign_idL = 0
        for hand_landmarks, handedness in zip(results.multi_hand_landmarks,
                                                results.multi_handedness):
            # Bounding box calculation
            brect = calc_bounding_rect(debug_image, hand_landmarks)
            # Landmark calculation
            landmark_list = calc_landmark_list(debug_image, hand_landmarks)

            # Conversion to relative coordinates / normalized coordinates
            pre_processed_landmark_list = pre_process_landmark(
                landmark_list)
            pre_processed_point_history_list = pre_process_point_history(
                debug_image, point_history)
            # Write to the dataset file
            #logging_csv(number, mode, pre_processed_landmark_list,
            #            pre_processed_point_history_list)

            # Hand sign classification
            hand_sign_id = keypoint_classifier(pre_processed_landmark_list)
            if hand_sign_id == 2:  # Point gesture
                point_history.append(landmark_list[8])
            else:
                point_history.append([0, 0])

            # Finger gesture classification
            finger_gesture_id = 0
            point_history_len = len(pre_processed_point_history_list)
            if point_history_len == (history_length * 2):
                finger_gesture_id = point_history_classifier(
                    pre_processed_point_history_list)

            # Calculates the gesture IDs in the latest detection
            finger_gesture_history.append(finger_gesture_id)
            most_common_fg_id = Counter(
                finger_gesture_history).most_common()

            # Drawing part
            debug_image = draw_bounding_rect(use_brect, debug_image, brect)
            debug_image = draw_landmarks(debug_image, landmark_list)
            debug_image = draw_info_text(
                debug_image,
                brect,
                handedness,
                keypoint_classifier_labels[hand_sign_id],
                point_history_classifier_labels[most_common_fg_id[0][0]],
            )

            # Make data available for processing
            # Assuming `classification` is the object you want to inspect
            index = handedness.classification[0].index

            if index == 1:
                palmR = landmark_list[0]
                indexR = landmark_list[8]
                indexRootR = landmark_list[5]
                thumbR = landmark_list[4]
            elif index == 0:
                palmL = landmark_list[0]
                indexL = landmark_list[8]
                thumbL = landmark_list[4]

        # Collecting data to send to Unity
        if CheckTriangle(thumbL, indexL, thumbR, indexR, indexRootR, hand_sign_idR, hand_sign_idL):
            X,Y = getCenter(thumbL, thumbR, indexL, indexR)
            transform = defineAxis()
            scalars = calculateRelative(X, Y, transform)
            msg = "0," + str(round(scalars[0],3)) + "," + str(round(scalars[1],3))
            print(msg)
            sendData(msg)

    else:
        point_history.append([0, 0])
        # Clear history to avoid lignering effects
        palmR = []
        palmL = []
        indexR = []
        indexRootR = []
        indexL = []
        thumbR = []
        thumbL = []

    debug_image = draw_point_history(debug_image, point_history)
    debug_image = draw_info(debug_image, fps)

    # Screen reflection #############################################################
    cv2.imshow('TermiteTerminator: Hand & Marker Recognition', debug_image)

############################################################################################
################################# Fiducial Marker Tracking #################################
############################################################################################

# construct the argument parser and parse the arguments
ap = argparse.ArgumentParser()
ap.add_argument("-t", "--type", type=str,
	default="DICT_ARUCO_ORIGINAL",
	help="type of ArUCo tag to detect")
args = vars(ap.parse_args())

# define names of each possible ArUco tag OpenCV supports
ARUCO_DICT = {
	"DICT_4X4_50": cv2.aruco.DICT_4X4_50,
	"DICT_4X4_100": cv2.aruco.DICT_4X4_100,
	"DICT_4X4_250": cv2.aruco.DICT_4X4_250,
	"DICT_4X4_1000": cv2.aruco.DICT_4X4_1000,
	"DICT_5X5_50": cv2.aruco.DICT_5X5_50,
	"DICT_5X5_100": cv2.aruco.DICT_5X5_100,
	"DICT_5X5_250": cv2.aruco.DICT_5X5_250,
	"DICT_5X5_1000": cv2.aruco.DICT_5X5_1000,
	"DICT_6X6_50": cv2.aruco.DICT_6X6_50,
	"DICT_6X6_100": cv2.aruco.DICT_6X6_100,
	"DICT_6X6_250": cv2.aruco.DICT_6X6_250,
	"DICT_6X6_1000": cv2.aruco.DICT_6X6_1000,
	"DICT_7X7_50": cv2.aruco.DICT_7X7_50,
	"DICT_7X7_100": cv2.aruco.DICT_7X7_100,
	"DICT_7X7_250": cv2.aruco.DICT_7X7_250,
	"DICT_7X7_1000": cv2.aruco.DICT_7X7_1000,
	"DICT_ARUCO_ORIGINAL": cv2.aruco.DICT_ARUCO_ORIGINAL,
#	"DICT_APRILTAG_16h5": cv2.aruco.DICT_APRILTAG_16h5,
#	"DICT_APRILTAG_25h9": cv2.aruco.DICT_APRILTAG_25h9,
#	"DICT_APRILTAG_36h10": cv2.aruco.DICT_APRILTAG_36h10,
#	"DICT_APRILTAG_36h11": cv2.aruco.DICT_APRILTAG_36h11
}

# verify that the supplied ArUCo tag exists and is supported by
# OpenCV
if ARUCO_DICT.get(args["type"], None) is None:
	print("[INFO] ArUCo tag of '{}' is not supported".format(
		args["type"]))
	sys.exit(0)

# load the ArUCo dictionary and grab the ArUCo parameters
print("[INFO] detecting '{}' tags...".format(args["type"]))
arucoDict = cv2.aruco.Dictionary_get(ARUCO_DICT[args["type"]])
arucoParams = cv2.aruco.DetectorParameters_create()

# Absolute coordinates of TV Fiducials
TV = np.array([[0, 0], [0, 0], [0, 0]])
TVready = np.array([0, 0, 0])

# List of all markers relative position [ID,X,Y]
towers = np.array([[1,0.0,0.0,0,0.0], [2,0.0,0.0,0,0.0], [3,0.0,0.0,0,0.0], [4,0.0,0.0,0,0.0], [5,0.0,0.0,0,0.0], [6,0.0,0.0,0,0.0], [7,0.0,0.0,0,0.0], [8,0.0,0.0,0,0.0], [9,0.0,0.0,0,0.0], [10,0.0,0.0,0,0.0]])

# Define and draw axis system to transform absolute to relative coordinates (bunch of math hoohaa)
def defineAxis():
	x_axis = (TV[2] - TV[0])
	y_axis = (TV[1] - TV[0])

	transform = np.array ([x_axis, y_axis])
	return transform

def calculateRelative(absoluteX, absoluteY, transform):
	vector = ([absoluteX - TV[0][0], absoluteY - TV[0][1]])
	scalars = np.linalg.solve(transform, vector)
	return scalars

def getLength(topLeftX, topLeftY, bottomRightX, bottomRightY):
	X = topLeftX - bottomRightX
	Y = topLeftY - bottomRightY
	distance = np.sqrt(np.square(X) + np.square(Y))
	return distance

def checkLifted(refLength, length):
	scale = length/refLength
	print(scale)
	if(scale > 1.35):
		return 1
	else:
		return 0

def getMarkerAngle(angle):
    # Returns the angle in degrees
	x_axis = (TV[2] - TV[0])
	y_axis = (TV[1] - TV[0])

	u = np.array(angle)
	x = np.array(x_axis)
	y = np.array(y_axis)

	# Positive or negative angle
	cos_thetaY = np.dot(u, y) / (np.linalg.norm(u) * np.linalg.norm(y))
	angleY_rad = np.arccos(np.clip(cos_thetaY, -1.0, 1.0))
	angleY_deg = np.degrees(angleY_rad)

	cos_thetaX = np.dot(u, x) / (np.linalg.norm(u) * np.linalg.norm(x))
	angleX_rad = np.arccos(np.clip(cos_thetaX, -1.0, 1.0))
	angleX_deg = np.degrees(angleX_rad)

	if (angleY_deg > 90):
		return 360 - angleX_deg
	else:
		return angleX_deg

def MarkerMain(frame, refMarkerSize):
    # grab the frame from the threaded video stream and resize it
	# to have a maximum width of 600 pixels
	#frame = vs.read()
	#frame = imutils.resize(frame, width=1000)

	# detect ArUco markers in the input frame
	(corners, ids, rejected) = cv2.aruco.detectMarkers(frame,
		arucoDict, parameters=arucoParams)

	# verify *at least* one ArUco marker was detected
	if len(corners) > 0:
		# flatten the ArUco IDs list
		ids = ids.flatten()

		# loop over the detected ArUCo corners
		for (markerCorner, markerID) in zip(corners, ids):
			# extract the marker corners (which are always returned
			# in top-left, top-right, bottom-right, and bottom-left
			# order)
			corners = markerCorner.reshape((4, 2))
			(topLeft, topRight, bottomRight, bottomLeft) = corners

			# convert each of the (x, y)-coordinate pairs to integers
			topRight = (int(topRight[0]), int(topRight[1]))
			bottomRight = (int(bottomRight[0]), int(bottomRight[1]))
			bottomLeft = (int(bottomLeft[0]), int(bottomLeft[1]))
			topLeft = (int(topLeft[0]), int(topLeft[1]))

			# draw the bounding box of the ArUCo detection
			cv2.line(frame, topLeft, topRight, (0, 255, 0), 2)
			cv2.line(frame, topRight, bottomRight, (0, 255, 0), 2)
			cv2.line(frame, bottomRight, bottomLeft, (0, 255, 0), 2)
			cv2.line(frame, bottomLeft, topLeft, (0, 255, 0), 2)

			# compute and draw the center (x, y)-coordinates of the
			# ArUco marker
			cX = int((topLeft[0] + bottomRight[0]) / 2.0)
			cY = int((topLeft[1] + bottomRight[1]) / 2.0)
			cv2.circle(frame, (cX, cY), 4, (0, 0, 255), -1)
			center = ([cX, cY])

			# draw the ArUco marker ID on the frame
			cv2.putText(frame, str(markerID),
				(topLeft[0], topLeft[1] - 15),
				cv2.FONT_HERSHEY_SIMPLEX,
				0.5, (0, 255, 0), 2)
			
			cv2.putText(frame, str(cX) +  str(cY),
				(bottomRight[0], bottomRight[1] - 15),
				cv2.FONT_HERSHEY_SIMPLEX,
				0.5, (0, 255, 0), 2)

			# Set TV corner coordinates
			if markerID == 21:
				TV[0] = np.array([cX, cY])
				TVready[0] = 1
				refMarkerSize = getLength(int(topLeft[0]), int(topLeft[1]), int(bottomRight[0]), int(bottomRight[1]))
				#print("Origin set at x=" + str(cX) + " y=" + str(cY))
			elif markerID == 22:
				TV[1] = np.array([cX, cY])
				TVready[1] = 1
				#print("Y axis set at x=" + str(cX) + " y=" + str(cY))
			elif markerID == 23:
				TV[2] = np.array([cX, cY])
				TVready[2] = 1
				#print("X axis set at x=" + str(cX) + " y=" + str(cY))

			# Transform absolute cX, xY to relative coordinates
			if np.array_equal(TVready, np.array([1, 1, 1])):
				# Draw axis information
				cv2.line(frame, (TV[0][0],TV[0][1]), (TV[1][0],TV[1][1]) , (0, 0, 255), 4,8,0)
				cv2.putText(frame, "Y-axis",
                                		(TV[1][0],TV[1][1]),
                                		cv2.FONT_HERSHEY_SIMPLEX,
                                		0.5, (0, 0, 255), 2)
				cv2.line(frame, (TV[0][0],TV[0][1]), (TV[2][0],TV[2][1]) , (255, 0, 0), 4,8,0)
				cv2.putText(frame, "X-axis",
                                		(TV[2][0],TV[2][1]),
                                		cv2.FONT_HERSHEY_SIMPLEX,
                                		0.5, (255, 0, 0), 2)
				
				if markerID < 20:
					transform = defineAxis()
					absoluteCoordinates = np.array([cX,cY])

					scalars = calculateRelative(cX, cY, transform)
					#print("Tower " + str(markerID) + " located at x=" + str(round(scalars[0],3)) + " y=" + str(round(scalars[1],3)))
					
					# Check if tower is in screen or not
					#if scalars[0] > 1 or scalars[1] > 1:
					#	print("Tower " + str(markerID) + "is out of the game area")

					# Draw Relative Coordinates
					cv2.putText(frame, str(round(scalars[0],3))+";"+str(round(scalars[1],3)),
                                		(cX, cY),
                                		cv2.FONT_HERSHEY_SIMPLEX,
                                		0.5, (255, 255, 255), 2)
					
					# Angle logic
					vTR = ([topRight[0] - cX, topRight[1] - cY])
					vBR = ([bottomRight[0] - cX, bottomRight[1] - cY])
					angle = ([vTR[0] + vBR[0], vTR[1] + vBR[1]])
					drawAngle = (angle[0] + cX, angle[1] + cY)
					cv2.line(frame, center, drawAngle, (255, 0, 255), 2)

					# Check if the tower is lifted
					markerSize = getLength(topLeft[0], topLeft[1], bottomRight[0], bottomRight[1])
					lifted = checkLifted(refMarkerSize, markerSize)
					
				# Add markers to list of id, TVcX, TVcY, lifted
					towers[markerID-1][1] = round(scalars[0],2)
					towers[markerID-1][2] = round(scalars[1],2)
					towers[markerID-1][3] = lifted
					towers[markerID-1][4] = round(getMarkerAngle(angle),0)*0.1

		# Pass data to unity
		if np.array_equal(TVready, np.array([1, 1, 1])):
			print("Sending Data to Unity:")
			print(str(towers))
			sendData(str(towers))
			print("Succesfully Sent Data to Unity")

		if refMarkerSize is not None:
			return refMarkerSize

	# show the output frame
	#cv2.imshow("Frame", frame)

############################################################################################
###################################### Combined Main #######################################
############################################################################################

def main():
    refMarkerSize = None
    while True:
        frame = shared_video_stream.read()
        if frame is None:
            break
        refMarkerSize = MarkerMain(frame, refMarkerSize)
        if np.array_equal(TVready, np.array([1, 1, 1])):
            GestureMain(frame)
        key = cv2.waitKey(1) & 0xFF
        if key == ord("q"):
            break
    # cleanup
    cap.release()
    cv2.destroyAllWindows()

if __name__ == '__main__':
    main()
