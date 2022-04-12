using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetection : MonoBehaviour
{
    Vector3 dir;
    Transform player;
    public LayerMask ignore;

    private void Start()
    {
        player = GameObject.Find("player").transform;
    }

    private void Update()
    {
        dir = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z) - new Vector3(gameObject.transform.parent.position.x, 1.5f, transform.parent.position.z);
        Debug.DrawRay(transform.parent.position, dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.tag == "Player")
        {
            RaycastHit hit;
            Physics.Raycast(transform.parent.position, dir, out hit, 100f , ~ignore);
            Debug.Log(hit.transform.gameObject.layer +" " + hit.transform.name);
            if(hit.transform.tag == "Player")
            {
                gameObject.transform.GetComponentInParent<zombie>().playerDetected = true;
                
            }
            
        }
    }
}
