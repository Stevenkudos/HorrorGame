using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class WallPerson : MonoBehaviour
{
    public Text dialogue;
    public string[] dialoguesBefore;
    public string[] dialoguesAfter;
    private bool haveMeat;
    public Collider box;
    public GameObject key;
    private int index;
    private int index2;

    private bool ran;

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

    public void HasMeat()
    {
        if (!ran)
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

            ran = true;
            StartCoroutine(EndDialogue());
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!haveMeat)
            {
                if (index < dialoguesBefore.Length)
                {
                    dialogue.text = "Voice in the wall: " + dialoguesBefore[index];
                    index++;
                }
                else
                {
                    index = 0;
                    dialogue.text = "Voice in the wall: " +dialoguesBefore[index];
                }
            }
            else
            {
                if (index2< dialoguesAfter.Length)
                {
                    dialogue.text = "Voice in the wall: " +dialoguesAfter[index];
                    index2++;
                }
            }
        }
        StartCoroutine(EndDialogue());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            dialogue.text = "";
    }

    IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(3f);
        dialogue.text = "";
        ran = false;
    }

    IEnumerator SpondKey()
    {
        foodDoor.SetTrigger("Give");
        yield return new WaitForSeconds(0.6f);
        foodDoor.SetTrigger("Give");
        yield return new WaitForSeconds(0.2f);
        Instantiate(key, transform.position, quaternion.identity);
    }
}
