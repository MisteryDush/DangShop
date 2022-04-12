using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieSounds : MonoBehaviour
{
    AudioSource audio;
    public AudioClip zombieStep;
    void Start()
    {
        audio = gameObject.transform.GetComponent<AudioSource>();
    }


    void Step()
    {
        audio.PlayOneShot(zombieStep);
    }
}
