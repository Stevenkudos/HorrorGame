using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderBench : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<FirstPersonController>().UnderBench(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player.GetComponent<FirstPersonController>().UnderBench(false);
    }
}
