using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PlayerDetection : MonoBehaviour
{
    public float detectRadius;
    public float detectDistance;
    public LayerMask layerMaskPlayer;
    public LayerMask layerMaskCover;
    public GameObject player;
    RaycastHit hit, hit1, hit2, hit3, hit4, hit5, hit6, hit7, hit8;
    RaycastHit cvrRayHit;

    public float terrorRadius;

    public bool isInSight;
    bool isTooClose;
    public bool isHiding;
    bool isGettingChased;
    float aggroCountDown;

    private bool isDead;
    // Start is called before the first frame update
    void Awake()
    {
        ChangeRadius(10f);
    }

    private void FixedUpdate()
    {
        DeathTrigger();
        PlayerDetector();
        CoverDetection();
    }

    void PlayerDetector()
    {
        if (!isHiding)
        {
            if (Physics.SphereCast(transform.position, detectRadius, transform.forward, out hit, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward - transform.right).normalized, out hit1, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward + transform.right).normalized, out hit2, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward - transform.up).normalized, out hit3, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward + transform.up).normalized, out hit4, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward - transform.right / 2).normalized, out hit5, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward + transform.right / 2).normalized, out hit6, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward - transform.up / 2).normalized, out hit7, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                Physics.SphereCast(transform.position, detectRadius, (transform.forward + transform.up / 2).normalized, out hit8, detectDistance, layerMaskPlayer, QueryTriggerInteraction.UseGlobal) ||
                isTooClose)
            {
                isInSight = true;
            }
            else isInSight = false;
        }
        else if(isTooClose)
        {
            isInSight = false;
            if (aggroCountDown >= -2)
                aggroCountDown -= 0.1f * Time.deltaTime;
        }

        if (aggroCountDown <= 0.1)
        {
            OnLosingAggro();
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, terrorRadius, layerMaskPlayer, QueryTriggerInteraction.UseGlobal);
        if (cols.Length >= 1)
        {
            isTooClose = true;
        }
        else isTooClose = false;

        if (isInSight)
            OnChase();

        //print(aggroCountDown);
    }

    public void ChangeRadius(float radius)
    {
        terrorRadius = radius;
    }

    void OnChase()
    {
        aggroCountDown = 0.6f;
        GetComponent<GoulWondering>().SetState(GoulWondering.State.CHASING);
        isGettingChased = true;
    }

    void OnLosingAggro()
    {
        GetComponent<GoulWondering>().SetState(GoulWondering.State.WANDERING);
        isGettingChased = false;
    }

    void CoverDetection()
    {
        isHiding = Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out cvrRayHit, Mathf.Infinity, layerMaskCover);
    }

    void DeathTrigger()
    {
        float playerDist = Vector3.Distance(transform.position, player.transform.position);
        if (playerDist <= 2.8f && !isHiding)
            isDead = true;
        if (isDead)
        {
            player.GetComponent<FirstPersonController>().OnDeath();
            Vector3 dir = player.transform.position - transform.position;
            Quaternion lookDir = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * 16);
        }
    }

    private void OnDrawGizmos()
    {
        if (!isTooClose)
        {
            if (isGettingChased)
            {
                if(isHiding) Gizmos.color = Color.yellow; else Gizmos.color = Color.red;
            }
            else if (!isGettingChased) Gizmos.color = Color.green;
        }
        else Gizmos.color = Color.black;

        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward - transform.right).normalized * hit1.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward - transform.right).normalized * hit1.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward + transform.right).normalized * hit2.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward + transform.right).normalized * hit2.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward - transform.up).normalized * hit3.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward - transform.up).normalized * hit3.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward + transform.up).normalized * hit4.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward + transform.up).normalized * hit4.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward - transform.right / 2).normalized * hit5.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward - transform.right / 2).normalized * hit5.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward + transform.right / 2).normalized * hit6.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward + transform.right / 2).normalized * hit6.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward - transform.up / 2).normalized * hit7.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward - transform.up / 2).normalized * hit7.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (transform.forward + transform.up / 2).normalized * hit8.distance);
        Gizmos.DrawWireSphere(transform.position + (transform.forward + transform.up/2).normalized * hit8.distance, detectRadius);

        Gizmos.DrawRay(transform.position, (player.transform.position - transform.position).normalized * cvrRayHit.distance);

        Gizmos.DrawWireSphere(transform.position, terrorRadius);
    }
}
