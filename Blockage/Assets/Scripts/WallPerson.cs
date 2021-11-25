using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.XR;

public class WallPerson : MonoBehaviour
{
    public Text dialogue;
    public string[] dialoguesBefore;
    public string[] dialoguesAfter;
    private bool haveMeat;
    public GameObject key;
    private int index;
    private int index2;
    public bool playerHasMeat;
    public bool begged;
    public AudioManager myMuse;
    private bool isSpeaking;

    public Animator foodDoor;
    // Start is called before the first frame update
    void Start()
    {
        dialogue.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecieveMeat()
    {
        StartCoroutine(SpondKey());
        haveMeat = true;
    }

    public void HasMeat(bool y)
    {
        if (!isSpeaking)
        {
            if (y)
            {
                if (index2< dialoguesAfter.Length)
                {
                    dialogue.text = "Voice in the wall: " + dialoguesAfter[index2];
                    index2++;
                }
                else
                {
                    dialogue.text = "[No response.]";
                }
            }
            else
            {
                if (!haveMeat)
                {
                    if (playerHasMeat)
                    {
                        if (!begged)
                        {
                            dialogue.text = "Voice in the wall: " + "Oh yes that ! can I have that please ?";
                            begged = true;
                        }
                        else 
                            dialogue.text = "Voice in the wall: " + "[ whimpering softly ]";
                    }
                    else
                    {
                        if (index < dialoguesBefore.Length)
                        {
                            dialogue.text = "Voice in the wall: " + dialoguesBefore[index];
                            index++;
                        }
                        else
                        {
                            dialogue.text = "Voice in the wall: " + "Hungry.... so hungry.....";
                        }
                    }
                }
            }
            isSpeaking = true;
            StartCoroutine(EndDialogue());
        }
    }
    public void OnTrigger()
    {
        if (!isSpeaking)
        {
            isSpeaking = true;
            if (!haveMeat)
            {
                if (playerHasMeat)
                {
                    if (!begged)
                    {
                        dialogue.text = "Voice in the wall: " + "Oh yes that! can I have that please ?";
                        begged = true;
                    }
                    else 
                        dialogue.text = "Voice in the wall: " + "[ whimpering softly ]";
                }
                else
                {
                    if (index < dialoguesBefore.Length)
                    {
                        dialogue.text = "Voice in the wall: " + dialoguesBefore[index];
                        index++;
                    }
                    else
                    {
                        dialogue.text = "Voice in the wall: " + "Hungry.... so hungry.....";
                    }
                }
            }
            else
            {
                if (index2< dialoguesAfter.Length)
                {
                    dialogue.text = "Voice in the wall: " + dialoguesAfter[index];
                    index2++;
                }
            }
            StartCoroutine(EndDialogue());
        }
    }

    IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(5f);
        dialogue.text = "";
        isSpeaking = false;
    }

    public Transform dropPos;
    IEnumerator SpondKey()
    {
        foodDoor.GetComponent<Collider>().enabled = false;
        isSpeaking = true;
        yield return new WaitForSeconds(0.2f);
        foodDoor.SetTrigger("Give");
        myMuse.Play("Chew");
        yield return new WaitForSeconds(4f);
        foodDoor.SetTrigger("Give");
        yield return new WaitForSeconds(0.2f);
        Instantiate(key, dropPos.position, quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        dialogue.text = "Voice in the wall: " + "Thank you...";
        yield return new WaitForSeconds(5f);
        dialogue.text = "";
        isSpeaking = false;
        foodDoor.GetComponent<Collider>().enabled = true;
    }
}
