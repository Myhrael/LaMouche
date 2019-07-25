using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float maxAngleSpeed = 2;
    public float angleSpeedGain = 1;
    public Vector3 vForward = new Vector3(0, 0, 1);

    public float angleSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        float yaw = Input.GetAxis("Horizontal");
        float pitch = Input.GetAxis("Vertical");
        float roll = Input.GetAxis("Roll");

        if((yaw!=0 || pitch!=0 || roll != 0) && angleSpeed<maxAngleSpeed)
        {
            angleSpeed += angleSpeedGain * dt;
            if (angleSpeed > maxAngleSpeed) angleSpeed = maxAngleSpeed;
        }else if (angleSpeed > 1)
        {
            angleSpeed -= angleSpeedGain * dt;
            if (angleSpeed < 1) angleSpeed = 1;
        }

        transform.Rotate(pitch*angleSpeed, yaw*angleSpeed, roll*angleSpeed);
        transform.Translate(vForward*dt);

        //transform.Translate(transform.rotation * dt * speed);
    }
}
