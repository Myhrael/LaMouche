using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2;
    public float maxAngleSpeed = 2;
    public float angleSpeedGain = 1;
    public float landDistance = 2;
    public float landMargin = 0.05f;
    public Cinemachine.CinemachineVirtualCamera landCam;
    public Camera mainCam;
    public Sprite deathSprite;

    private bool hitValid = false;
    private Vector3 landPoint;
    private Vector3 landNormal;
    private Transform landTransform;
    private Animator animator;
    private Vector3 vForward = new Vector3(0, 0, 1);
    private LandingManager approachScript;
    private float angleSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        approachScript = GetComponentInChildren<LandingManager>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("flying", true);
        animator.SetBool("landed", false);
    }

    // Update is called once per frame
    void Update()
    {
        bool isFlying = animator.GetBool("flying");
        if (isFlying)
        {
            float dt = Time.deltaTime;

            float yaw = Input.GetAxis("Horizontal");
            float pitch = Input.GetAxis("Vertical");
            float roll = Input.GetAxis("Roll");

            if ((yaw != 0 || pitch != 0 || roll != 0) && angleSpeed < maxAngleSpeed)
            {
                angleSpeed += angleSpeedGain * dt;
                if (angleSpeed > maxAngleSpeed) angleSpeed = maxAngleSpeed;
            }
            else if (angleSpeed > 1)
            {
                angleSpeed -= angleSpeedGain * dt;
                if (angleSpeed < 1) angleSpeed = 1;
            }

            transform.Rotate(pitch * angleSpeed, yaw * angleSpeed, roll * angleSpeed);
            transform.Translate(vForward * dt * speed);
        }

        detectLanding();

        if (Input.GetKeyDown("space"))
        {
            if (isFlying && hitValid)
            {
                animator.SetBool("flying", false);
                landCam.LookAt = landTransform;
                approachScript.approach(landPoint, landNormal);
            }
            else if (!isFlying)
            {
                takeOff();
            }
        }
    }

    public void die()
    {
        Debug.Log("You died mthfckr!!");
        GetComponentInChildren<SpriteRenderer>().sprite = deathSprite;
        animator.enabled = false;
        animator.SetBool("landed", false);

        GameManager.gm.lost();
    }

    public void fly()
    {
        animator.SetBool("flying", true);
    }

    public void land()
    {
        animator.SetBool("landed", true);
    }

    public void takeOff()
    {
        animator.SetBool("landed", false);
        GetComponentInChildren<LandingManager>().transform.rotation = transform.rotation;
        approachScript.returnToCam(landCam.transform.position);
    }

    public Transform getLandObject()
    {
        return landTransform;
    }

    private void detectLanding()
    {
        RaycastHit hit;
        Vector3 dir = transform.GetChild(0).GetChild(0).position - transform.position;
        dir = dir.normalized;
        Ray ray = new Ray(transform.position, dir);
        int layerMask = 1 << 9;

        if (Physics.Raycast(ray, out hit, landDistance, layerMask))
        {
            landPoint = hit.point-dir*landMargin;
            landNormal = hit.normal;
            landTransform = hit.collider.transform;
            hitValid = true;
        }
        else
        {
            hitValid = false;
        }
    }
}
