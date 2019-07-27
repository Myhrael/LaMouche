using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCam : MonoBehaviour
{
    public Transform cam;
    public Transform forward;
    public float rotationThreshold = 5f;
    public float rotationTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vForward = Vector3.forward; //(forward.position - transform.position).normalized;
        Vector3 target = (cam.position - transform.position).normalized;

        if (Vector3.Angle(vForward, target) >= rotationThreshold)
        {
            Quaternion rotation = Quaternion.FromToRotation(vForward, target);

            transform.rotation = rotation; //Quaternion.Slerp(transform.rotation, transform.rotation * rotation, rotationTime);
        }
    }
}
