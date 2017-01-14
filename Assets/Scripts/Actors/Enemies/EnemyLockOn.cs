using UnityEngine;
using System.Collections;

// Lock on, but for enemies
public class EnemyLockOn : MonoBehaviour
{
    [SerializeField]
    GameObject lockOnReticle;               // The reticle used for determining if there is a lock on
    private Animator reticleAnimator;       // The animator for the reticle

    [HideInInspector]
    public GameObject nearestEnemy = null;
    private int enemyList;
    [HideInInspector]
    public Quaternion defaultRotation;
    [HideInInspector]
    public bool facingRight = true;

    // Use this for initialization
    void Awake()
    {
        if (lockOnReticle != null)
        {
            reticleAnimator = lockOnReticle.GetComponent<Animator>();
        }
        defaultRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        GetLockOn();
        if (nearestEnemy != null)
        {
            CheckFacingRight();
            ChangeRotation();
            if (lockOnReticle != null)
            {
                lockOnReticle.transform.position = nearestEnemy.transform.position;
            }
        }
    }

    // Acquires a lock on if there is a suitable target
    public void GetLockOn()
    {
        float nearestDistance = Mathf.Infinity;

        // If lock on is not disabled and there is a valid target, lock on
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            float distance = (transform.position - obj.transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                if (lockOnReticle != null)
                {
                    if (lockOnReticle.GetComponent<SpriteRenderer>().color.a < 0.534f)
                    {
                        reticleAnimator.SetBool("Intro", true);
                        Invoke("DisableIntroAnim", 0.3f);
                    }
                }
                nearestDistance = distance;
                nearestEnemy = obj;
            }
        }

        if (lockOnReticle != null)
        {
            AdjustLockOnReticle();
        }
    }

    void AdjustLockOnReticle()
    {
        // If locked on and not locking on to the same enemy, adjust the lock on reticle
        if (nearestEnemy != null && nearestEnemy.transform.parent == true)
        {
            lockOnReticle.transform.position = nearestEnemy.transform.position;
            // Plays audio
            if (lockOnReticle.GetComponent<SpriteRenderer>().color.a < 0.528f)
            {
                if (lockOnReticle.GetComponent<AudioSource>() != null)
                {
                    lockOnReticle.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    // Invoke functions that disable the animation boolean
    void DisableIntroAnim()
    {
        reticleAnimator.SetBool("Intro", false);
    }
    void DisableExitAnim()
    {
        reticleAnimator.SetBool("Exit", false);
    }

    void ChangeRotation()
    {
        if (nearestEnemy != null)
        {
            Vector3 vectorToTarget = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 999999);

            if (!facingRight)
            {
                transform.rotation *= Quaternion.Euler(180f, 0, 0);
            }
        }
    }

    // Check if target is to the right
    void CheckFacingRight()
    {
        if (transform.position.x > nearestEnemy.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }
}
