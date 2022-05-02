using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoCrate : MonoBehaviour
{
    private int _ammoCapacity;
    [SerializeField] private Gun weapon;
    private int[] ammoProbability = { 5, 5, 5, 6, 6, 6, 7, 7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 10, 10 }; 

    void RandomizeAmmo()
    {
        var index = Random.Range(0, ammoProbability.Length);
        _ammoCapacity = ammoProbability[index];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            RandomizeAmmo();
            weapon.ammoLeft += _ammoCapacity;
            Destroy(gameObject);
        }
    }
}
