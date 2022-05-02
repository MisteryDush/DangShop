using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pathfinding : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public Transform player;
    private void Awake()
    {
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        _navMeshAgent.destination = player.position;
    }
}
