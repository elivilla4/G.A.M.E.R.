from keras.preprocessing.image import img_to_array
import imutils
import cv2
from keras.models import load_model
import numpy as np
import os
import random
import socket
import struct
import traceback
import logging
import time

# parameters for loading data and images
detection_model_path = 'haarcascade_files/haarcascade_frontalface_default.xml'
emotion_model_path = 'models/_mini_XCEPTION.102-0.66.hdf5'

# hyper-parameters for bounding boxes shape
# loading models
face_detection = cv2.CascadeClassifier(detection_model_path)
emotion_classifier = load_model(emotion_model_path, compile=False)
EMOTIONS = ["angry" ,"disgust","scared", "happy", "sad", "surprised",
 "neutral"]

current_emotion = None
delay = 10

# starting video streaming
cv2.namedWindow('your_face')
camera = cv2.VideoCapture(0)

def sending_and_reciveing():
    s = socket.socket()
    socket.setdefaulttimeout(None)
    print('socket created ')
    port = 60000
    s.bind(('127.0.0.1', port)) #local host
    s.listen(30) #listening for connection for 30 sec?
    print('socket listensing ... ')
    while True:
    
        # show camera feed with bounding box
        #cv2.imshow('your_face', frameClone)

        try:
            c, addr = s.accept() # waiting for port connection
            bytes_received = c.recv(4000) #received bytes
            array_received = np.frombuffer(bytes_received, dtype=np.float32) #converting into float array

            # reading the frame
            frame = camera.read()[1]
            frame = imutils.resize(frame,width=300)
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
            faces = face_detection.detectMultiScale(gray,scaleFactor=1.1,minNeighbors=5,minSize=(30,30),flags=cv2.CASCADE_SCALE_IMAGE)

            # set bounding box
            nn_output = np.array([])
            frameClone = frame.copy()
            if len(faces) > 0:
                faces = sorted(faces, reverse=True,
                key=lambda x: (x[2] - x[0]) * (x[3] - x[1]))[0]
                (fX, fY, fW, fH) = faces

                # Extract the ROI of the face from the grayscale image, resize it to a fixed 28x28 pixels, and then prepare
                # the ROI for classification via the CNN
                roi = gray[fY:fY + fH, fX:fX + fW]
                roi = cv2.resize(roi, (64, 64))
                roi = roi.astype("float") / 255.0
                roi = img_to_array(roi)
                roi = np.expand_dims(roi, axis=0)
                
                
                preds = emotion_classifier.predict(roi)[0]
                nn_output = preds
                label = EMOTIONS[preds.argmax()]
                current_emotion = label

                # cv2.putText(frameClone, label, (fX, fY - 10),
                #         cv2.FONT_HERSHEY_SIMPLEX, 0.45, (0, 0, 255), 2)
                # cv2.rectangle(frameClone, (fX, fY), (fX + fW, fY + fH),
                #         (0, 0, 255), 2)

            bytes_to_send = struct.pack('%sf' % len(nn_output), *nn_output) #converting float to byte
            cumulative_predictions = np.zeros(7) # reset cumulative array
            c.sendall(bytes_to_send) #sending back
            c.close()
        except Exception as e:
            logging.error(traceback.format_exc())
            print("error")
            c.sendall(bytearray([]))
            c.close()
            break

sending_and_reciveing()

camera.release()
cv2.destroyAllWindows()