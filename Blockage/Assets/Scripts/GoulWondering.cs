using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoulWondering : MonoBehaviour
{
    public List<GameObject> Centers;

    public float WanderRadius;

    public NavMeshAgent agent;

    public enum State { WANDERING, CHASING, INVESTIGATING}
    public State cur_state;

    bool inChase;

    Transform player;

    private void Awake()
    {
        player = GameObject.Find("FirstPersonController").transform;
    }
    private void Update()
    {
        //print(hasReachedDestination());
        switch (cur_state)
        {
            default:
            case State.WANDERING:
                Wandering();
                break;
            case State.CHASING:
                Chasing();
                break;
        }
    }

    public void SetState(State newState)
    {
        cur_state = newState;
        //print(cur_state);
    }

    private bool HasReachedDestination()
    {
        return agent.remainingDistance <= agent.stoppingDistance || inChase;
    }

    public void Wandering()
    {
        inChase = false;
        agent.speed = 3.5f;
        if (!HasReachedDestination()) return;
        GameObject newCenter = Centers[Random.Range(0, Centers.Count)];
        agent.SetDestination(RandomPosition(newCenter));
    }

    public void Chasing()
    {
        agent.speed = 4.8f;
        agent.destination = player.position;
        inChase = true;
    }

    private Vector3 RandomPosition(GameObject center)
    {
        var randDirection = Random.insideUnitSphere * WanderRadius;

        randDirection += center.transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, WanderRadius, 1);

        return navHit.position;
    }
    
}
