using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cabinets : MonoBehaviour
{
    private bool isOpen;
    private Vector3 originPos;
    public Vector3 openedPos;

    public GameObject objectInnit;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        openedPos = transform.position + Vector3.forward/2;
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(transform.position, openedPos, Time.deltaTime * 2);
            StartCoroutine(UVsS());
        }
    }

    public void OnCabinet()
    {
        isOpen = true;
    }

    IEnumerator UVsS()
    {
        yield return new WaitForSeconds(0.2f);
        if (objectInnit != null)
        {
            objectInnit.GetComponent<Collider>().enabled = true;
        }
    }
    
}
