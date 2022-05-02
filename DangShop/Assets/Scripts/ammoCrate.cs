using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoCrate : MonoBehaviour
{
    int ammoCapacity;
    [SerializeField] private Gun weapon;
    private int[] ammo = { 5, 5, 5, 6, 6, 6, 7, 7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 10, 10 }; 

    void RandomizeAmmo()
    {
        var index = Random.Range(0, ammo.Length);
        ammoCapacity = ammo[index];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            RandomizeAmmo();
            weapon.ammoLeft += ammoCapacity;
            Destroy(gameObject);
        }
    }
}
