using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColliderKey : MonoBehaviour
{
    public GameObject hip;
    public GameObject player;
    private Vector3 offseted;
    // Start is called before the first frame update
    void Awake()
    {
        offseted = transform.position - hip.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = hip.transform.rotation;
        if (player.GetComponent<FirstPersonController>().isStunned)
        {
            transform.position = (hip.transform.position + offseted) + Vector3.up * .5f;
        }else
            transform.position = hip.transform.position + offseted;
    }
    void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(collision.collider,GetComponent<Collider>());
    }
}
