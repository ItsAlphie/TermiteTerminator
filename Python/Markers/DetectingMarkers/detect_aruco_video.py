#!/usr/bin/env python

# import the necessary packages
from imutils.video import VideoStream
import argparse
import imutils
import time
import cv2
import sys
import numpy as np
import socket
import time

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

# Requirements for TCP communication to Unity
# sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
ip, port = "127.0.0.1", 11069
# sock.connect((host, port))

def sendData(data):	
	sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
	sock.sendto(data.encode(), (ip, port))

# Absolute coordinates of TV Fiducials
TV = np.array([[0, 0], [0, 0], [0, 0]])
TVready = np.array([0, 0, 0])

# List of all markers relative position [ID,X,Y]
towers = np.array([[1,0.0,0.0,0], [2,0.0,0.0,0], [3,0.0,0.0,0], [4,0.0,0.0,0], [5,0.0,0.0,0], [6,0.0,0.0,0], [7,0.0,0.0,0], [8,0.0,0.0,0], [9,0.0,0.0,0], [10,0.0,0.0,0]])

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
	if(scale > 1):
		return 1
	else:
		return 0

# initialize the video stream and allow the camera sensor to warm up
print("[INFO] starting video stream...")
vs = VideoStream(src=1).start()
time.sleep(2.0)

# loop over the frames from the video stream
while True:
	# grab the frame from the threaded video stream and resize it
	# to have a maximum width of 600 pixels
	frame = vs.read()
	frame = imutils.resize(frame, width=1000)

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

					# Check if the tower is lifted
					markerSize = getLength(int(topLeft[0]), int(topLeft[1]), int(bottomRight[0]), int(bottomRight[1]))
					lifted = checkLifted(refMarkerSize, markerSize)
					
				# Add markers to list of id, TVcX, TVcY, lifted
					towers[markerID-1][1] = round(scalars[0],2)
					towers[markerID-1][2] = round(scalars[1],2)
					towers[markerID-1][3] = lifted

		# Pass data to unity
		if np.array_equal(TVready, np.array([1, 1, 1])):
			print("Sending Data to Unity:")
			print(str(towers))
			sendData(str(towers))
			print("Succesfully Sent Data to Unity")

	# show the output frame
	cv2.imshow("Frame", frame)
	key = cv2.waitKey(1) & 0xFF
	print("Sleeping")
	time.sleep(0.5)
	print("Woke up")

	# if the `q` key was pressed, break from the loop
	if key == ord("q"):
		break

# do a bit of cleanup
cv2.destroyAllWindows()
vs.stop()
