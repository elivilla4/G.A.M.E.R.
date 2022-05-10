using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinController : MonoBehaviour
{
    Animator animator;
    Renderer[] characterMaterials;

    public Texture2D[] albedoList;
    [ColorUsage(true,true)]
    public Color[] eyeColors;
    public enum EyePosition { normal, happy, angry, dead}
    public EyePosition eyeState;

    public GameObject eyes;
    public Material eyeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterMaterials = GetComponentsInChildren<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //ChangeMaterialSettings(0);
            ChangeEyeOffset(EyePosition.normal);
            ChangeAnimatorIdle("normal");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //ChangeMaterialSettings(1);
            ChangeEyeOffset(EyePosition.angry);
            ChangeAnimatorIdle("angry");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //ChangeMaterialSettings(2);
            ChangeEyeOffset(EyePosition.happy);
            ChangeAnimatorIdle("happy");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //ChangeMaterialSettings(3);
            ChangeEyeOffset(EyePosition.dead);
            ChangeAnimatorIdle("dead");
        }
    }

    void ChangeAnimatorIdle(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    void ChangeMaterialSettings(int index)
    {
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
                characterMaterials[i].material.SetColor("_EmissionColor", eyeColors[index]);
            else
                characterMaterials[i].material.SetTexture("_MainTex",albedoList[index]);
        }
    }

    void ChangeEyeOffset(EyePosition pos)
    {
        Vector2 offset = Vector2.zero;

        switch (pos)
        {
            case EyePosition.normal:
                offset = new Vector2(0, 0);
                break;
            case EyePosition.happy:
                offset = new Vector2(.33f, 0);
                break;
            case EyePosition.angry:
                offset = new Vector2(.66f, 0);
                break;
            case EyePosition.dead:
                offset = new Vector2(.33f, .66f);
                break;
            default:
                break;
        }

        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
                characterMaterials[i].material.SetTextureOffset("_MainTex", offset);
        }
    }

    public void ChangeFace(string emotion) {
        if (emotion == "happy") {
            ChangeEyeOffset(EyePosition.happy);
            ChangeAnimatorIdle("happy");
            //eyes.GetComponent<Renderer>().material.color = eyeColors[2];
            eyeMaterial.color = eyeColors[2];
        } else if (emotion == "mundane") {
            ChangeEyeOffset(EyePosition.normal);
            ChangeAnimatorIdle("normal");
            //eyes.GetComponent<Renderer>().material.color = eyeColors[0];
            eyeMaterial.color = eyeColors[0];
        } else {
            ChangeEyeOffset(EyePosition.angry);
            ChangeAnimatorIdle("angry");
            //eyes.GetComponent<Renderer>().material.color = eyeColors[1];
            eyeMaterial.color = eyeColors[1];
        }
    }

    public void Dead() {
        ChangeEyeOffset(EyePosition.dead);
        ChangeAnimatorIdle("dead");
        //eyes.GetComponent<Renderer>().material.color = eyeColors[1];
        eyeMaterial.color = eyeColors[1];
        GetComponent<MovementInput>().enabled = false;
    }
}
