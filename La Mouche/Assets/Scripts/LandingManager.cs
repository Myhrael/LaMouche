using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingManager : MonoBehaviour
{
    public float spriteSpeed = 1.5f;
    public float goalThreshold = 0.08f;
    public float camThreshold = 0.5f;

    private bool returning;
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

            if((returning && positionApproximatelyEquals(transform.position, target, camThreshold))
                || positionApproximatelyEquals(transform.position, target, goalThreshold))
            {
                targetValid = false;

                if (returning)
                {
                    transform.position = parentTransform.position;
                    pc.fly();
                    returning = false;
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
    public void returnToCam(Vector3 target)
    {
        returning = true;
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
