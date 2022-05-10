from keras.preprocessing.image import img_to_array
import imutils
import cv2
from keras.models import load_model
import numpy as np
import pygame
import os
import random
pygame.font.init()
pygame.mixer.init()

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

WIDTH, HEIGHT = 900, 500
WIN = pygame.display.set_mode((WIDTH, HEIGHT)) # create a window
pygame.display.set_caption("First PyGame")

WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
RED = (255, 0, 0)
YELLOW = (255, 255, 0)

BORDER = pygame.Rect(WIDTH//2 - 5, 0, 10, HEIGHT)

FPS = 60

SCORE_FONT = pygame.font.SysFont('comicsans', 40)

# load sounds
BULLET_HIT_SOUND = pygame.mixer.Sound(os.path.join('Assets', 'Grenade+1.mp3'))
BULLET_FIRE_SOUND = pygame.mixer.Sound(os.path.join('Assets', 'Gun+Silencer.mp3'))

# player properties
VELOCITY = 5
SPACESHIP_WIDTH, SPACESHIP_HEIGHT = 55, 40
ASTEROID_WIDTH, ASTEROID_HEIGHT = 50, 50

# set codes for collision events
ASTEROID_HIT = pygame.USEREVENT + 1

# bullet properties
BULLET_VELOCITY = 20
BULLET_WIDTH, BULLET_HEIGHT = 10, 5
MAX_BULLETS = 3

# player player
SPACESHIP_IMAGE = pygame.image.load(os.path.join('Assets', 'spaceship_yellow.png'))
SPACESHIP = pygame.transform.rotate(pygame.transform.scale(SPACESHIP_IMAGE, (SPACESHIP_WIDTH, SPACESHIP_HEIGHT)), 90) # scale and rotate spaceship image

# asteroid
ASTEROID_IMAGE = pygame.image.load(os.path.join('Assets', 'asteroid.png'))
ASTEROID = pygame.transform.rotate(pygame.transform.scale(ASTEROID_IMAGE, (ASTEROID_WIDTH, ASTEROID_HEIGHT)), 270) # scale and rotate spaceship image

# background
#SPACE = pygame.transform.scale(pygame.image.load(os.path.join('Assets', 'space.png')), (WIDTH, HEIGHT))
HAPPY_SPACE = pygame.transform.scale(pygame.image.load(os.path.join('Assets', 'happy.jpeg')), (WIDTH, HEIGHT))

#feelings_faces = []
#for index, emotion in enumerate(EMOTIONS):
   # feelings_faces.append(cv2.imread('emojis/' + emotion + '.png', -1))

# starting video streaming
# cv2.namedWindow('your_face')
# camera = cv2.VideoCapture(0)
# while True:
#     frame = camera.read()[1]
#     #reading the frame
#     frame = imutils.resize(frame,width=300)
#     gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
#     faces = face_detection.detectMultiScale(gray,scaleFactor=1.1,minNeighbors=5,minSize=(30,30),flags=cv2.CASCADE_SCALE_IMAGE)
    
#     canvas = np.zeros((250, 300, 3), dtype="uint8")
#     frameClone = frame.copy()
#     if len(faces) > 0:
#         faces = sorted(faces, reverse=True,
#         key=lambda x: (x[2] - x[0]) * (x[3] - x[1]))[0]
#         (fX, fY, fW, fH) = faces
#                     # Extract the ROI of the face from the grayscale image, resize it to a fixed 28x28 pixels, and then prepare
#             # the ROI for classification via the CNN
#         roi = gray[fY:fY + fH, fX:fX + fW]
#         roi = cv2.resize(roi, (64, 64))
#         roi = roi.astype("float") / 255.0
#         roi = img_to_array(roi)
#         roi = np.expand_dims(roi, axis=0)
        
        
#         preds = emotion_classifier.predict(roi)[0]
#         emotion_probability = np.max(preds)
#         label = EMOTIONS[preds.argmax()]
#         current_emotion = label
#     else: continue

 
#     for (i, (emotion, prob)) in enumerate(zip(EMOTIONS, preds)):
#                 # construct the label text
#                 text = "{}: {:.2f}%".format(emotion, prob * 100)

#                 # draw the label + probability bar on the canvas
#                # emoji_face = feelings_faces[np.argmax(preds)]

                
#                 w = int(prob * 300)
#                 cv2.rectangle(canvas, (7, (i * 35) + 5),
#                 (w, (i * 35) + 35), (0, 0, 255), -1)
#                 cv2.putText(canvas, text, (10, (i * 35) + 23),
#                 cv2.FONT_HERSHEY_SIMPLEX, 0.45,
#                 (255, 255, 255), 2)
#                 cv2.putText(frameClone, label, (fX, fY - 10),
#                 cv2.FONT_HERSHEY_SIMPLEX, 0.45, (0, 0, 255), 2)
#                 cv2.rectangle(frameClone, (fX, fY), (fX + fW, fY + fH),
#                               (0, 0, 255), 2)

    
# #    for c in range(0, 3):
# #        frame[200:320, 10:130, c] = emoji_face[:, :, c] * \
# #        (emoji_face[:, :, 3] / 255.0) + frame[200:320,
# #        10:130, c] * (1.0 - emoji_face[:, :, 3] / 255.0)


#     cv2.imshow('your_face', frameClone)
#     cv2.imshow("Probabilities", canvas)
#     if cv2.waitKey(1) & 0xFF == ord('q'):
#         break


def draw_window(player, bullets, asteroid, score):
    # draw background
    if current_emotion == "happy":
        WIN.blit(HAPPY_SPACE, (0,0))
    else:
        WIN.fill(BLACK)

    # draw health text
    score_text = SCORE_FONT.render("Score: " + str(score), 1, WHITE)
    WIN.blit(score_text, (10, 10))

    # draw spaceship
    WIN.blit(SPACESHIP, (player.x, player.y)) # blit used to display images on screen

    # draw asteroid
    WIN.blit(ASTEROID, (asteroid.x, asteroid.y))
    
    # draw bullets
    for bullet in bullets:
        pygame.draw.rect(WIN, YELLOW, bullet)

    pygame.display.update()

def handle_player_movement(keys_pressed, player):
    if keys_pressed[pygame.K_LEFT] and player.x - VELOCITY > 0: # LEFT
        player.x -= VELOCITY
    if keys_pressed[pygame.K_RIGHT] and player.x + player.width + VELOCITY < BORDER.x: # RIGHT
        player.x += VELOCITY
    if keys_pressed[pygame.K_UP] and player.y - VELOCITY > 0: # UP
        player.y -= VELOCITY
    if keys_pressed[pygame.K_DOWN] and player.y + player.height + VELOCITY < HEIGHT - 15: # DOWN
        player.y += VELOCITY

def handle_bullets(asteroid, bullets):
    for bullet in bullets[:]:
        bullet.x += BULLET_VELOCITY
        if asteroid.colliderect(bullet): # player bullet collided with red character
            pygame.event.post(pygame.event.Event(ASTEROID_HIT))
            bullets.remove(bullet)
        elif bullet.x > WIDTH:
            bullets.remove(bullet)

# starting video streaming
cv2.namedWindow('your_face')
camera = cv2.VideoCapture(0)

def main(): 
    '''
    handle the main game loop
    '''
    global current_emotion

    player = pygame.Rect(100, 300, SPACESHIP_WIDTH, SPACESHIP_HEIGHT)
    asteroid = pygame.Rect(random.randint(650, 800), random.randint(50, 400), ASTEROID_WIDTH, ASTEROID_HEIGHT)
    
    bullets = []

    score = 0

    clock = pygame.time.Clock()
    
    run = True
    while run:
        clock.tick(FPS)
        frame = camera.read()[1]

        #reading the frame
        frame = imutils.resize(frame,width=300)
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        faces = face_detection.detectMultiScale(gray,scaleFactor=1.1,minNeighbors=5,minSize=(30,30),flags=cv2.CASCADE_SCALE_IMAGE)
        
        canvas = np.zeros((250, 300, 3), dtype="uint8")
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
            emotion_probability = np.max(preds)
            label = EMOTIONS[preds.argmax()]
            current_emotion = label

            cv2.putText(frameClone, label, (fX, fY - 10),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.45, (0, 0, 255), 2)
            cv2.rectangle(frameClone, (fX, fY), (fX + fW, fY + fH),
                    (0, 0, 255), 2)
        #else: continue

    
        # for (i, (emotion, prob)) in enumerate(zip(EMOTIONS, preds)):
        #             # construct the label text
        #             text = "{}: {:.2f}%".format(emotion, prob * 100)

        #             # draw the label + probability bar on the canvas
        #         # emoji_face = feelings_faces[np.argmax(preds)]

                    
        #             w = int(prob * 300)
        #             cv2.rectangle(canvas, (7, (i * 35) + 5),
        #             (w, (i * 35) + 35), (0, 0, 255), -1)
        #             cv2.putText(canvas, text, (10, (i * 35) + 23),
        #             cv2.FONT_HERSHEY_SIMPLEX, 0.45,
        #             (255, 255, 255), 2)
        #             cv2.putText(frameClone, label, (fX, fY - 10),
        #             cv2.FONT_HERSHEY_SIMPLEX, 0.45, (0, 0, 255), 2)
        #             cv2.rectangle(frameClone, (fX, fY), (fX + fW, fY + fH),
        #                         (0, 0, 255), 2)
        
    #    for c in range(0, 3):
    #        frame[200:320, 10:130, c] = emoji_face[:, :, c] * \
    #        (emoji_face[:, :, 3] / 255.0) + frame[200:320,
    #        10:130, c] * (1.0 - emoji_face[:, :, 3] / 255.0)

        cv2.imshow('your_face', frameClone)
        # cv2.imshow("Probabilities", canvas)

        for event in pygame.event.get():
            # check all events 
            if event.type == pygame.QUIT:
                # user quit the game
                run = False
                pygame.quit()
            
            if event.type == pygame.KEYDOWN: # key was pressed down
                if event.key == pygame.K_SPACE and len(bullets) < MAX_BULLETS:
                    bullet = pygame.Rect(player.x + player.width, player.y + player.height//2 - BULLET_HEIGHT // 2, BULLET_WIDTH, BULLET_HEIGHT) # spawn bullet in front of the player player
                    bullets.append(bullet)
                    BULLET_FIRE_SOUND.play()

            if event.type == ASTEROID_HIT:
                asteroid.x, asteroid.y = random.randint(650, 800), random.randint(50, 400)
                score += 1
                BULLET_HIT_SOUND.play()
            
        keys_pressed = pygame.key.get_pressed() # get all keys that are pressed down
        handle_player_movement(keys_pressed, player)

        handle_bullets(asteroid, bullets)

        draw_window(player, bullets, asteroid, score)

    main()

main()
camera.release()
cv2.destroyAllWindows()

