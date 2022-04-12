using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("1")) SelectWeapon(0);
        else if (Input.GetKeyDown("2")) SelectWeapon(1);
    }

    void SelectWeapon(int i)
    {
        foreach(Transform weapon in transform)
        {
            if (i == weapon.GetSiblingIndex())
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                if (weapon.GetComponent<Gun>() != null)
                {
                    weapon.GetComponent<Gun>().SwitchFromPistol();
                }
                weapon.gameObject.SetActive(false);
            }
        }
    }
}
