using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Camera cam;
    float pickRange = 2f;
    private Gun weapon;
    Bottle bottle;
    public GameObject mk;
    playerStats heal;
    [SerializeField] private AudioClip startRunning;
    private AudioSource audioSource;

    private void Start()
    {
        heal = gameObject.GetComponent<playerStats>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }


    void Update()
    {
        InputDetection();
    }

    void InputDetection()
    {
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            transform.position += transform.right * -speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            transform.position += transform.forward * -speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKeyDown("e"))
        {
            RaycastHit obj;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out obj, pickRange);
            try
            {
                weapon = obj.transform.GetComponent<Gun>();
                bottle = obj.transform.GetComponent<Bottle>();
            }
            catch (System.NullReferenceException e)
            {

            }
            if (weapon != null)
            {
                weapon.PickedUp();
            }
            if(bottle != null)
            {
                bottle.PickedUp();
            }
        }
        if (Input.GetKeyDown("q"))
        {
            if (heal.hp < 100f)
            {
                StopCoroutine(heal.Heal());
                StartCoroutine(heal.Heal());
            }
        }
            if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 9f;
            audioSource.Stop();
            audioSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5f;
        }

    }

}
