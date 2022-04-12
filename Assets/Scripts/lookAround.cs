using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAround : MonoBehaviour
{
    Vector2 mD;
    Transform player;
    [SerializeField] private Transform gun;
    public float sens = 3f;
    private void Start()
    {
        player = gameObject.transform.parent.transform;
    }
    void Update()
    {
        Vector2 mC = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mD += mC;
        if (mD.y <= 90f)
        {
            this.transform.localRotation = Quaternion.AngleAxis(-mD.y, Vector3.right);
        }
        else
        {
            mD.y = 90f;
        }
        if (mD.y >= -75f)
        {
            this.transform.localRotation = Quaternion.AngleAxis(-mD.y, Vector3.right);
        }
        else
        {
            mD.y = -75f;
        }
        player.localRotation = Quaternion.AngleAxis(mD.x, Vector3.up);
    }
}
