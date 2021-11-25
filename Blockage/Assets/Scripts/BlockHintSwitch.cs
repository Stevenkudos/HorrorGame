using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHintSwitch : MonoBehaviour
{
    public GameObject hint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hint.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hint.SetActive(true);
        }
    }
}
