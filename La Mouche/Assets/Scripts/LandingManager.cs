using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingManager : MonoBehaviour
{
    public float spriteSpeed = 0.8f;

    private bool targetValid = false;
    private Vector3 target;
    private Vector3 landNormal;
    private PlayerController pc;
    private SpriteRenderer spriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetValid)
        {
            Vector3 dir = target - transform.position;
            Transform parentTransform = transform.parent;

            if(positionApproximatelyEquals(transform.position, target, 0.005f))
            {
                targetValid = false;

                if (target.Equals(parentTransform.position))
                {
                    transform.position = parentTransform.position;
                    pc.fly();
                }
                else
                {
                    Transform forwardTransform = transform.GetChild(0);
                    Vector3 vForward = forwardTransform.position - transform.position;
                    transform.rotation *= Quaternion.FromToRotation(vForward, -landNormal);

                    pc.land();
                }
            }
            else
            {
                transform.Translate(dir * Time.deltaTime, Space.World);
            }
        }
    }

    public void approach(Vector3 target, Vector3 normal)
    {
        this.target = target;
        landNormal = normal;
        targetValid = true;
    }
    public void approach(Vector3 target)
    {
        this.target = target;
        targetValid = true;
    }

    private bool positionApproximatelyEquals(Vector3 t1, Vector3 t2, float threshold)
    {
        if (Mathf.Abs(t1.x - t2.x) >= threshold) return false;
        if (Mathf.Abs(t1.y - t2.y) >= threshold) return false;
        if (Mathf.Abs(t1.z - t2.z) >= threshold) return false;

        return true;
    }
}
