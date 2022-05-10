using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float healthRecharge = 10f;
    public float depletionRate = 2.5f; // in health per second

    private float health;
    private bool playerAlive;

    public Text jammoText;
    public Text healthText;
    private Color defaultTextColor;
    private Color intenseTextColor = Color.red;
    private AudioSource rechargeAudio;
    private AudioSource deathAudio;

    public GameObject environmentHandler;
    public CharacterSkinController characterSkinControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerAlive = true;
        health = maxHealth;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        rechargeAudio = audioSources[0];
        deathAudio = audioSources[1];
        defaultTextColor = healthText.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAlive) {
            DecreaseHealth();
            healthText.text = "Battery: " + health.ToString("0.0") + "%";
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Battery") {

            Destroy(collider.gameObject);
            rechargeAudio.Play();

            health += healthRecharge;
            if (health > maxHealth) {
                health = maxHealth;
            }
        }
    }

    void DecreaseHealth() {
        health -= depletionRate * Time.deltaTime;
        if (health <= 0) {
            health = 0;
            EndGame();
        }
    }

    void EndGame() {
        playerAlive = false;
        environmentHandler.GetComponent<EnvironmentHandler>().Dead();
        characterSkinControllerScript.Dead();
        deathAudio.Play();
        healthText.color = Color.red;
        jammoText.color = Color.red;
        StartCoroutine("Restart");
    }

    private IEnumerator Restart() {
        yield return new WaitForSeconds(deathAudio.clip.length / 2);
        SceneManager.LoadScene("JammoScene");
    }

    public void SetDepletionRate(float newDepletionRate, bool isDefaultRate) {
        depletionRate = newDepletionRate;
        if (isDefaultRate) {
            healthText.color = defaultTextColor;
            jammoText.color = defaultTextColor;
        } else {
            healthText.color = intenseTextColor;
            jammoText.color = intenseTextColor;
        }
    }
}
