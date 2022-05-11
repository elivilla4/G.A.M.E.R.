# G.A.M.E.R.
# Table of Contents :
1.[Description](#p1)

2.[Installations](#p2)

3.[Usage](#p3)

4.[Dataset](#p4)


<a id="p1"></a> 
# Description:

G.A.M.E.R. is a system that uses emotion recognition to change the state of a game based on how the player is feeling. G.A.M.E.R. uses the player’s emotion to create a more personalized gaming experience that aims to make thee game more enjoyable. The system takes image data from a live camera feed and determines the player’s emotional state using an emotion recognition model that is pretrained on the FER-2013 dataset. 

<a id="p2"></a> 
# Installations:

This repository contains files from the Emotion-recognition repository, which can be found [here](https://github.com/omar178/Emotion-recognition). Following the instructions on that page should allow you to run the necessary files for this project. Particularly, installing the dependencies using requirements.txt should be enough.

```shell
pip install -r requirements.txt
```

The Unity game for this project was made using Unity version 2020.3.32f. Having this Unity editor version is required, and downloading Unity Hub [here](https://unity3d.com/get-unity/download) will allow you to open the project. 

<a id="p3"></a> 
# Usage:

There are two versions of the project in this repository. Version 1 is the version from the Prototype studio, while Version 3 is the final version that was submitted and showcaseed at the Final Studio. The following are instructions for how this project was run on macOS Monterey 12.2.1 and Python version 3.10.2.

## Version 1

This is the first version of this project, which uses pygame. In order to run this version, you will need to install pygame.

```shell
pip install pygame
```

This program will display two windows. The first will show the live web camera feed with a bounding box for the face and a label for the current emotion being detected. The second contains a simple game where the player controls a spaceship shooting asteroids. This simply changes the background of the game whenever the "happy" emotion is detected. The following command will run the program

```shell
python real_time_video.py
```

## Version 3

This is the final version of the project, which uses both Python and Unity. You will need to open the 'Unity Emotion Game' project from this repo in Unity (through Unity Hub). Once the Unity project is open, use the 'Project' tab in the Unity editor to open the 'Scenes' folder. Open the 'JammoScene' scene if it is not already open. 

<a id="p4"></a> 
# Dataset:

I have used [this](https://www.kaggle.com/c/3364/download-all) dataset

Download it and put the csv in fer2013/fer2013/

-fer2013 emotion classification test accuracy: 66%


# Credits
This work is inspired from [this](https://github.com/oarriaga/face_classification) great work and the resources of Adrian Rosebrock helped me alot!.

# Ongoing 
Draw emotions faces next to the detected face.

# Issues & Suggestions

If any issues and suggestions to me, you can create an [issue](https://github.com/omar178/Emotion-recognition/issues).

If you like this work please help me by giving me some stars.
