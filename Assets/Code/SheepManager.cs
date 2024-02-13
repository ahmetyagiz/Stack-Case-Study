using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 randomPos;
    private bool isSetRandomPos;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PatrolMovement();
    }

    void PatrolMovement()
    {
        Invoke(nameof(SetRandomPos), 0.5f);

        navMeshAgent.SetDestination(randomPos);

        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    isSetRandomPos = false;
                    SetRandomPos();
                }
            }

        }
    }

    void SetRandomPos()
    {
        if (isSetRandomPos == false)
        {
            randomPos = new Vector3(Random.Range(-4.5f, -1), 2, Random.Range(0f, 11f));

            isSetRandomPos = true;
        }
    }
}