using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAway : MonoBehaviour
{
    public GameObject paint;
    public GameObject doorBehind;

    private float zPos;
    // Start is called before the first frame update
    void Start()
    {
        zPos = transform.position.z - 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (paint.activeSelf)
        {
            Move();
            doorBehind.isStatic = false;
        }
    }

    public void Move()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,transform.position.y,zPos), Time.deltaTime);
    }
}
