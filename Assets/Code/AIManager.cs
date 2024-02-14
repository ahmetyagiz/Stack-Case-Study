using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform machine_1;
    [SerializeField] private Transform machine_2;
    private Transform currentDestination;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentDestination = machine_1;
    }

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        navMeshAgent.SetDestination(currentDestination.position);

        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (currentDestination == machine_1)
                    {
                        currentDestination = machine_2;
                    }
                    else if (currentDestination == machine_2)
                    {
                        currentDestination = machine_1;
                    }
                }
            }
        }
    }
}