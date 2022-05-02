using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombie : MonoBehaviour
{
    public Animator anim;
    private bool _setRagdoll = false;
    private bool _setCollider = true;
    private Rigidbody[] _rigs;
    private Collider[] _cols;
    public float hp = 3;
    public bool dead = false;
    public Transform player;
    private float rotationSpeed = 3f;
    private float _speed = 3f;
    public float distance;
    private const float MinDist = 1.7f;
    private bool _canWalk = true;
    private CharacterController _cc;
    private new AudioSource _audio;
    public AudioClip[] deathSounds;
    public AudioClip audioShot;
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;
    public float radius = 10f;
    public float angle = 90f;
    public LayerMask targetMask;
    public bool playerDetected = false;
    public MeshCollider fov;
    private float wanderingRadius = 10f;
    private float wanderingInterval = 5f;
    private float _wanderCooldown = 0f;
    private bool _isWarned = false;
    private float walkingSpeed = 1f;
    private float runningSpeed = 3.5f;
    private static readonly int Hit = Animator.StringToHash("hit");


    private void Start()
    {
        _audio = gameObject.transform.GetComponent<AudioSource>();
        _cc = transform.GetComponent<CharacterController>();
        _rigs = gameObject.GetComponentsInChildren<Rigidbody>();
        _cols = gameObject.GetComponentsInChildren<Collider>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _navMeshObstacle = gameObject.GetComponent<NavMeshObstacle>();
        _navMeshObstacle.enabled = false;
        foreach (Rigidbody rig in _rigs)
        {
            rig.isKinematic = _setRagdoll;
        }

        foreach (Collider col in _cols)
        {
            col.enabled = _setCollider;
            col.isTrigger = false;
        }
        fov.isTrigger = true;
    }

    private void Update()
    {
        if (playerDetected) WhatToDo();
        else if (!playerDetected && !_isWarned) WanderAround();
    }
    public void TakeDamage(float damage)
    {
        StopAllCoroutines();
        playerDetected = true;
        _audio.PlayOneShot(audioShot);
        hp -= damage;
        anim.SetTrigger(Hit);
        StartCoroutine(WaitForWalk(0.967f));
        if (hp <= 0)
        {
            _setRagdoll = false;
            _setCollider = true;
            anim.enabled = false;
            Die();    
        }
    }

    private void Die()
    {
        _navMeshAgent.enabled = false;
        if (hp == 0)
        {
            AudioClip deadSound = deathSounds[Random.Range(0, deathSounds.Length)];
            _audio.PlayOneShot(deadSound);
            StartCoroutine(Wait());
        }
        dead = true;
        foreach(Rigidbody rig in _rigs)
        {
            rig.isKinematic = _setRagdoll;
        }

        foreach(Collider col in _cols)
        {
            col.enabled = _setCollider;
        }     
    }

    private void LookAtPlayer()
    {
        if (!dead && playerDetected)
        {
            Vector3 relPos = player.position - transform.position;
            relPos.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relPos), rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale -= new Vector3(transform.localScale.x * 0.5f * Time.deltaTime,
                transform.localScale.y * 0.5f * Time.deltaTime,
                transform.localScale.z * 0.5f * Time.deltaTime);
        }
    }

    private void WhatToDo()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        _navMeshAgent.speed = runningSpeed;
        Vector3 direction = player.transform.position - gameObject.transform.position;
        if (distance > MinDist && !dead && _canWalk)
        {
            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = true;
            _navMeshAgent.destination = player.position;
            anim.SetBool("isWalking", true);
        }
        else if (distance < MinDist && !dead)
        {
            LookAtPlayer();
            _navMeshAgent.enabled = false;
            _navMeshObstacle.enabled = true;
            anim.SetBool("isWalking", false);
            anim.SetTrigger("attack");
            StartCoroutine(WaitForWalk(2.367f));
        }
    }

    public void SoundHeard(Transform origin)
    {
        if (playerDetected) return;
        _isWarned = true;
        StopCoroutine(CheckSound(origin));
        StartCoroutine(CheckSound(origin));

    }

    private void WanderAround()
    {
        if (_navMeshAgent.velocity.sqrMagnitude == 0f)
        {
            anim.SetBool("isSlowWalking", false);
        }
        else
        {
            anim.SetBool("isSlowWalking", true);
        }
        if (_wanderCooldown < Time.time)
        {
            var destination = RandomNavMeshLocation();
            distance = Vector3.Distance(destination, gameObject.transform.position);
            _navMeshAgent.speed = walkingSpeed;
            _navMeshAgent.destination = destination;
            _wanderCooldown = Time.time + wanderingInterval;
        }
    }

    private Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPos = Vector3.zero;
        Vector3 randomPos = Random.insideUnitSphere * wanderingRadius;
        randomPos += transform.position;
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, wanderingRadius, 1))
        {
            finalPos = hit.position; 
        }
        return finalPos;
    }


    private IEnumerator CheckSound(Transform origin)
    {
        _navMeshAgent.destination = origin.position;
        _navMeshAgent.speed = walkingSpeed;
        //Debug.Log("Checking...");
        yield return new WaitUntil(() => _navMeshAgent.remainingDistance < 0.5f);
        //Debug.Log("Checked!");
        anim.SetBool("isSlowWalking", false);
        //Debug.Log("Waiting for 3s");
        yield return new WaitForSeconds(1f);
        //Debug.Log("Continue wandering");
        _isWarned = false;
    }

    private IEnumerator WaitForWalk(float len)
    {
        _navMeshAgent.enabled = false;
        _navMeshObstacle.enabled = true;
        _canWalk = false;
        yield return new WaitForSeconds(len);
        _canWalk = true;
        _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
