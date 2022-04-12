using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;
    private float xOffset = 1;
    private float yOffset = -1f;
    private float zOffset = 1f;

    private void Update()
    {
        var isPicked = gameObject.GetComponent<Gun>().IsPicked;
        var isShooting = gameObject.GetComponent<Gun>().IsShooting;
        var isReloading = gameObject.GetComponent<Gun>().IsReloading;
        if (isPicked && !isShooting && !isReloading )
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            Quaternion rotateX = Quaternion.AngleAxis(-mouseY, Vector3.up);
            Quaternion rotateY = Quaternion.AngleAxis(mouseX, Vector3.forward);

            Quaternion targetRotation = rotateX * rotateY;

            Debug.Log(Quaternion.identity + ", " + new Quaternion(targetRotation.x + xOffset, targetRotation.y + yOffset, targetRotation.z + zOffset, 1f));

            transform.localRotation = Quaternion.Lerp(Quaternion.identity ,new Quaternion(targetRotation.x + xOffset, targetRotation.y + yOffset, targetRotation.z + zOffset, 1f), smooth);
        }
    }
}
