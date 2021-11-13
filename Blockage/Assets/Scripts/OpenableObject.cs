using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableObject : MonoBehaviour
{
    public GameObject door;

    public GameObject loock;

    public void Unlock()
    {
        door.GetComponent<Rigidbody>().isKinematic = false;
        loock.GetComponent<Rigidbody>().isKinematic = false;
    }
}
