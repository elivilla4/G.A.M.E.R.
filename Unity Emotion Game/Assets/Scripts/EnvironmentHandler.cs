using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentHandler : MonoBehaviour
{

    public Material happyMaterial;
    public Material intenseMaterial;
    public Material mundaneMaterial;

    private AudioSource mundaneSong;
    private AudioSource happySong;
    private AudioSource intenseSong;

    private string currentCategory;
    private string currentEmotion;

    // rainbow colors: red, orange, yellow, green, blue, magenta
    private Color[] happyColors = {Color.red, Color.Lerp(Color.red, Color.yellow, 0.5f), Color.yellow, Color.green, Color.blue, Color.magenta};

    private int currentColorIndex;
    private float colorChangeDuratiion = 0.25f; 
    private float happyChangeTimer; 
    private float intenseChangeTimer;

    public Light playerLight;

    public MovementInput movementInputScript;
    public float defaultVelocity = 10f;
    public float happyVelocity = 15f;

    public PlayerHealth playerHealthScript;
    public float defaultDepletionRate;
    public float intenseDepletionRate;

    private float happyEffectDuration;
    private float intenseEffectDuration;
    private float mundaneEffectDuration = 5f;

    private bool happyEffectPlaying;
    private bool intenseEffectPlaying;

    public CharacterSkinController characterSkinController;
    public EmotionHandler emotionHandler;

    public Text emotionText;
    public float emotionTextDisplayDuration = 2f;
    private float emotionTextDisplayTimer;
    private bool displayText;

    private bool playerAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        happySong = audioSources[0];
        intenseSong = audioSources[1];
        mundaneSong = audioSources[2];
        currentCategory = "mundane";
        currentColorIndex = 0;
        happyEffectPlaying = false;
        intenseEffectPlaying = false;
        happyEffectDuration = happySong.clip.length;
        intenseEffectDuration = happySong.clip.length;
        displayText = false;
        SetMundane();
        UpdateEnvrionment("happy", "happy");
    }

    // Update is called once per frame
    void Update()
    {
        if (happyEffectPlaying) {
            HappyLight();
        } else if (intenseEffectPlaying) {
            IntenseLight();
        }

        if (displayText) {
            if (Time.time - emotionTextDisplayTimer < emotionTextDisplayDuration / 2) {
                // make the text fade in
                float fraction = (Time.time - emotionTextDisplayTimer) / (emotionTextDisplayDuration / 2);
                float a = Mathf.Lerp(0, 1, fraction);
                emotionText.color = new Color(emotionText.color.r, emotionText.color.g, emotionText.color.b, a);
            } else {
                // make the text fade out
                float fraction = (Time.time - emotionTextDisplayTimer - (emotionTextDisplayDuration / 2)) / (emotionTextDisplayDuration / 2);
                float a = Mathf.Lerp(1, 0, fraction);
                emotionText.color = new Color(emotionText.color.r, emotionText.color.g, emotionText.color.b, a);
                if (fraction > 1) {
                    displayText = false;
                }
            }
        }
    }

    public void UpdateEnvrionment(string emotionCategory, string emotion) {
        currentCategory = emotionCategory;
        currentEmotion = emotion;
        // if (!happyEffectPlaying && !intenseEffectPlaying) {
        //     switch (emotionCategory) {
        //         case "happy":
        //             SetHappy();
        //             break;
        //         case "intense":
        //             SetIntense();
        //             break;
        //         case "mundane":
        //             SetMundane();
        //             break;
        //     }
        // }
    }

    private void SetMundane() {
        if (playerAlive) {
            RenderSettings.skybox = mundaneMaterial;
            playerLight.color = Color.white;
            movementInputScript.SetVelocity(defaultVelocity);
            playerHealthScript.SetDepletionRate(defaultDepletionRate, true);
            characterSkinController.ChangeFace("mundane");
            
            happySong.Stop();
            intenseSong.Stop();
            mundaneSong.Play();

            //currentCategory = "mundane";
            StartCoroutine("EndMundane");
        }
    }

    private IEnumerator EndMundane() {
        yield return new WaitForSeconds(mundaneEffectDuration);
        //emotionHandler.GetCurrentEmotion();
        if (currentCategory == "happy") {
            SetHappy();
        } else if (currentCategory == "intentse") {
            SetIntense();
        } else {
            StartCoroutine("EndMundane");
        }
    }

    private void SetHappy() {
        if (playerAlive) {
            happyEffectPlaying = true;
            RenderSettings.skybox = happyMaterial;
            movementInputScript.SetVelocity(happyVelocity);
            playerHealthScript.SetDepletionRate(defaultDepletionRate, true);
            characterSkinController.ChangeFace("happy");

            intenseSong.Stop();
            mundaneSong.Stop();
            happySong.Play();

            happyChangeTimer = Time.time;
            //currentCategory = "happy";
            StartCoroutine("EndHappy");
        }
    }

    private IEnumerator EndHappy() {
        ShowEmotionText();
        yield return new WaitForSeconds(happyEffectDuration);
        happyEffectPlaying = false;
        emotionHandler.GetCurrentEmotion();
        if (currentCategory == "intense") {
            SetIntense();
        } else if (currentCategory == "mundane") {
            SetMundane();
        } else {
            StartCoroutine("EndHappy");
        }
    }

    private void SetIntense() {
        if (playerAlive) {
            intenseEffectPlaying = true;
            RenderSettings.skybox = intenseMaterial;        
            playerLight.color = Color.red;
            movementInputScript.SetVelocity(defaultVelocity);
            playerHealthScript.SetDepletionRate(intenseDepletionRate, false);
            characterSkinController.ChangeFace("intense");

            happySong.Stop();
            mundaneSong.Stop();
            intenseSong.Play();

            intenseChangeTimer = Time.time;
            //currentCategory = "intense";
            StartCoroutine("EndIntense");
        }
    }

    private IEnumerator EndIntense() {
        ShowEmotionText();
        yield return new WaitForSeconds(intenseEffectDuration);
        intenseEffectPlaying = false;
        emotionHandler.GetCurrentEmotion();
        if (currentCategory == "happy") {
            SetHappy();
        } else if (currentCategory == "mundane") {
            SetMundane();
        } else {
            StartCoroutine("EndIntense");
        }
    }

    private void HappyLight() {
        float fraction = (Time.time - happyChangeTimer) / (colorChangeDuratiion);
        playerLight.color = Color.Lerp(happyColors[currentColorIndex], happyColors[(currentColorIndex + 1) % happyColors.Length], fraction);
        if (fraction > 1) {
            currentColorIndex = (currentColorIndex + 1) % happyColors.Length;
            happyChangeTimer = Time.time;
        }
    }

    private void IntenseLight() {
        float fraction = Mathf.PingPong(Time.time, colorChangeDuratiion) / colorChangeDuratiion;
        playerLight.intensity = Mathf.Lerp(0, 1, fraction);
    }

    private void ShowEmotionText() {
        emotionText.text = "Jammo is " + currentEmotion.ToUpper() + "!";
        switch (currentEmotion) {
            case "angry":
                emotionText.color = Color.red;
                break;
            case "disgust":
                emotionText.color = Color.green;
                break;
            case "scared":
                emotionText.color = Color.black;
                break;
            case "happy":
                emotionText.color = Color.yellow;
                break;
            case "sad":
                emotionText.color = Color.blue;
                break;
            case "surprised":
                emotionText.color = Color.magenta;
                break;
            case "neutral":
                emotionText.color = Color.white;
                break;
        }
    displayText = true;
    emotionTextDisplayTimer = Time.time;
    }

    public void Dead() {
        playerAlive = false;
        happySong.Stop();
        intenseSong.Stop();
        mundaneSong.Stop();
        emotionText.text = "Jammo is Dead :(";
        emotionText.color = new Color(1, 0, 0, 1);
    }
}
