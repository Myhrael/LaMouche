using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroController : MonoBehaviour
{
    public Transform theFly;
    public Transform interactableObjectsRoot;
    public ParticleSystem hitParticle;
    public List<AudioClip> hitAudio = new List<AudioClip>(3);

    public float hitRange = 0.05f;
    public float nearbyRange = 4f;
    public float proximityTreshold = 1f;
    public float rangeIncrement = 1f;
    public float distTreshold = 8f;
    public float attackCooldown = 3;
    public float speed = 1.5f;

    private RotateTowardsCam rotationScript;
    private Animator animator;
    private Animator flyAnim;
    private List<Transform> interactableObjects = new List<Transform>();
    private Transform target;
    private Transform origin;

    private AudioSource hitAudioSource = new AudioSource();

    private bool canAttack = true;
    private bool timerEnabled = false;
    private float attackTimer;
    private float angryness = 0;

    // Start is called before the first frame update
    void Start()
    {
        //getInteractableObjects(interactableObjectsRoot);
        rotationScript = GetComponentInChildren<RotateTowardsCam>();
        flyAnim = theFly.GetComponentInChildren<Animator>();
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("angryness", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Ceil(angryness) > animator.GetInteger("angryness"))
        {
            animator.SetInteger("angryness", (int) Mathf.Ceil(angryness));
        }

        if (timerEnabled)
        {
            updateTimer();
        }

        Vector3 vFly = theFly.position - transform.position;

        if (!flyAnim.GetBool("flying"))
        {
            if (vFly.magnitude <= 2.5f)
            {
                rotationScript.enabled = false;
            }

            int mode = animator.GetInteger("angryness");
            origin = GetComponentInChildren<SpriteRenderer>().transform.GetChild(mode);
            Vector3 dir = theFly.position - origin.position;
            
            if (dir.magnitude > hitRange) target = theFly;
            else
            {
                target = null;

                if (flyAnim.GetBool("landed") && canAttack)
                {
                    animator.SetBool("attack", true);
                    canAttack = false;
                }
            }
        }
        else
        {
            rotationScript.enabled = true;

            if (vFly.magnitude >= distTreshold && target == null && false)
            {
                //Transform target = getNearbyObject(theFly, nearbyRange);
                origin = transform;
                Vector3 dir = theFly.position - origin.position;

                if (dir.magnitude >= proximityTreshold) target = theFly;
                else target = null;
            }
        }

        if(target != null)
        {
            if (!origin.position.Equals(target.position))
            {
                Vector3 targetPos = target.position + transform.position - origin.position;
                Vector3 dir = targetPos - transform.position;
                float realSpeed = dir.magnitude < 1 ? speed / 3 : dir.magnitude < 2 ? speed / 1.5f : speed;

                transform.position += dir.normalized * Time.deltaTime * speed;
                if (dir.magnitude <= hitRange) transform.position = targetPos;
            }
        }
    }

    public void endAttack()
    {
        //hitAudioSource.PlayOneShot(hitAudio[animator.GetInteger("angryness")], 0.7f);

        timerEnabled = true;
        Object.Instantiate(hitParticle, theFly.position, theFly.rotation);

        if (flyAnim.GetBool("landed") && (theFly.position - origin.position).magnitude <= hitRange)
        {
            animator.SetBool("gotTheKill", true);
            theFly.GetComponentInParent<PlayerController>().die();
        }

        DestroyObject objToDestroy = theFly.GetComponentInParent<PlayerController>().getLandObject().GetComponent<DestroyObject>();

        if (objToDestroy.level <= animator.GetInteger("angryness"))
        {
            if (objToDestroy.level == 2)
            {
                GameManager.gm.won();
            }
            angryness += objToDestroy.destroy();
        }
    }

    private void updateTimer()
    {
        attackTimer -= Time.deltaTime;

        if(attackTimer <= 0)
        {
            animator.SetBool("attack", false);
            canAttack = true;
            attackTimer = attackCooldown;
            timerEnabled = false;
        }
    }

    private void getInteractableObjects(Transform root)
    {
        if(root.transform.childCount == 0)
        {
            root.name = "object" + interactableObjects.Count;
            interactableObjects.Add(root);
        }
        else
        {
            foreach(Transform child in root.transform)
            {
                if(child != root.transform)
                {
                    getInteractableObjects(child);
                }
            }
        }
    }

    private Transform getNearbyObject(Transform center, float range)
    {
        List<Transform> nearbyObjects = new List<Transform>();

        foreach(Transform obj in interactableObjects)
        {
            float dist = (obj.position - theFly.position).magnitude;

            if (dist <= nearbyRange) nearbyObjects.Add(obj);
        }

        if (nearbyObjects.Count > 0)
        {
            int index = Random.Range(0, nearbyObjects.Count);
            return nearbyObjects[index];
        }
        else
        {
            //return getNearbyObject(center, range + rangeIncrement);
            return null;
        }
        
    }
}
