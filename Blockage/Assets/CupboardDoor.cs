using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardDoor : MonoBehaviour
{
    public GameObject Lock;
    public Collider itemBehind;
    public Collider door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Lock.GetComponent<Rigidbody>().isKinematic)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            if(itemBehind != null)
                itemBehind.enabled = true;
        }
    }


    void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Untagged"))
            {
                Physics.IgnoreCollision(collision.collider,door);
            }
        }
}
