using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoisonZombie : MonoBehaviour
{
        private NavMeshAgent _navMeshAgent;
        private NavMeshObstacle _navMeshObstacle;
        private Transform _playerTransform;
        [SerializeField] private Animator animator;
        private bool _setRagdoll = true;
        private bool _setCollider = true;
        private Rigidbody[] _rigs;
        private Collider[] _cols;


        private void Start()
        {
                _playerTransform = GameObject.Find("player").transform;
                _navMeshAgent = gameObject.transform.GetComponent<NavMeshAgent>();
                _navMeshObstacle = gameObject.transform.GetComponent<NavMeshObstacle>();
                _navMeshObstacle.enabled = false;
                _rigs = gameObject.GetComponentsInChildren<Rigidbody>();
                _cols = gameObject.GetComponentsInChildren<Collider>();
                foreach (Rigidbody rig in _rigs)
                {
                        rig.isKinematic = _setRagdoll;
                }

                foreach (Collider col in _cols)
                {
                        col.enabled = _setCollider;
                        col.isTrigger = false;
                }

                _navMeshObstacle.enabled = false;
                animator.SetBool("isWalking", true);
                
        }

        private void Update()
        {
                _navMeshAgent.destination = _playerTransform.position;
                var distance = Vector3.Distance(gameObject.transform.position, _playerTransform.position);
                if (distance < 5f)
                {
                        animator.SetBool("isWalking", false);
                        _navMeshAgent.enabled = false;
                }
                else
                {
                        animator.SetBool("isWalking", true);
                        _navMeshAgent.enabled = true;
                }
        }
}
