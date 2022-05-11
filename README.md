# G.A.M.E.R.
# Table of Content :
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

The program will creat a window to display the scene capture by webcamera and a window representing the probabilities of detected emotions.

> Demo

python real_time_video.py

You can just use this with the provided pretrained model i have included in the path written in the code file, i have choosen this specificaly since it scores the best accuracy, feel free to choose any but in this case you have to run the later file train_emotion_classifier
> If you just want to run this demo, the following content can be skipped
- Train

- python train_emotion_classifier.py


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
