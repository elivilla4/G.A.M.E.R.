using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionHandler : PythonCommunication
{
    private float startTime;
    public float delay = 0.5f;

    private string[] emotions = {"angry", "disgust", "scared", "happy", "sad", "surprised", "neutral"};
    private string[] emotionCategories = {"intense", "happy", "mundane"};
    private float[] currentEmotionCount = new float[] {0,0,0,0,0,0,0};
    private float[] totalEmotionCount = new float[] {0,0,0,0,0,0,0};
    private string currentEmotion;

    private int numEmotionsRecorded;
    private int emotionChangeNumber = 10;

    public EnvironmentHandler environmentHandler;
    public Text emotionText;

    // Start is called before the first frame update
    void Start()
    {
        numEmotionsRecorded = 0;
        startTime = 0;
    }

    void Update() {

        // if (Time.time - startTime > delay) {
        //     float[] input = {0f};
        //     float[] outputArray = ServerRequest(input);

        //     if (outputArray.Length > 0) { 
        //         int newEmotionIndex = GetEmotionIndex(outputArray);
        //         currentEmotionCount[newEmotionIndex] += 1;
        //         if (newEmotionIndex < 3 || newEmotionIndex == 5) {
        //             currentEmotionCount[newEmotionIndex] += 1000;
        //         }
        //         numEmotionsRecorded += 1;
        //         startTime = Time.time;
        //     }
        // }
    }

    public void GetCurrentEmotion() {
        UpdateEmotionCount();

        string emotion = GetEmotionLabel(currentEmotionCount);
        //emotionText.text = emotion;

        string emotionCategory = GetEmotionCategory(emotion);
        environmentHandler.UpdateEnvrionment(emotionCategory, emotion);

        currentEmotionCount = new float[] {0,0,0,0,0,0,0};
        numEmotionsRecorded = 0;

        startTime = Time.time;
    }

    int GetEmotionIndex(float[] predictions) {
        int maxIndex = 0;
        float maxValue = predictions[0];

        for (int i = 1; i < predictions.Length; i++) {
            if (predictions[i] > maxValue){
                maxIndex = i;
                maxValue = predictions[i];
            }
        }

        return maxIndex;
    }

    string GetEmotionLabel(float[] predictions) {
        return emotions[GetEmotionIndex(predictions)];
    }

    string GetEmotionCategory(string emotion) {
        if (emotion == "angry" || emotion == "disgust" || emotion == "scared" || emotion == "surprised") {
            return "intense";
        } else if (emotion == "happy") {
            return "happy";
        } else {
            return "mundane";
        }
    }

    void UpdateEmotionCount() {
        for (int i = 0; i < totalEmotionCount.Length; i++) {
            totalEmotionCount[i] += currentEmotionCount[i];
        }
    }
}
