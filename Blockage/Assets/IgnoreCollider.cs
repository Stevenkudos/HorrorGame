using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IgnoreCollider : MonoBehaviour
{
    public Transform player;
    public Transform ghoul;
    public Collider door;
    Animator animator;

    public Animation open2;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        door = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position,player.position) <= 7 && Vector3.Distance(transform.position,ghoul.position) > 3)
        {
            animator.enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            Physics.IgnoreCollision(collision.collider,door);
        }
    }

    public void Open()
    {
        animator.SetTrigger("Enter");
    }

}
