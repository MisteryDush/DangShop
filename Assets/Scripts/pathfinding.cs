using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pathfinding : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player;
    private void Awake()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        navMeshAgent.destination = player.position;
    }
}
