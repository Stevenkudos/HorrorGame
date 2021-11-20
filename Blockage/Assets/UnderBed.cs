using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderBed : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<FirstPersonController>().UnderBed(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player.GetComponent<FirstPersonController>().UnderBed(false);
    }
}
