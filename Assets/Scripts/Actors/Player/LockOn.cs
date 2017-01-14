using UnityEngine;
using System.Collections;

// LockOn class for the player
public class LockOn : MonoBehaviour
{
    public static LockOn current;
    [SerializeField]
    GameObject lockOnReticle;               // The reticle used for determining if there is a lock on
    private Animator reticleAnimator;       // The animator for the reticle

    private int enemyList;
    private Quaternion defaultRotation;
    private bool facingRight = true;
    [HideInInspector]
    public GameObject nearestEnemy = null;
    private GameObject previousEnemy = null;

    [HideInInspector]
    public bool shouldLockOn = true;       // If there is no lock on and a target enters the eligible range, lock on

    private float disableLockOn = 0.0f;     // Countdown to disable lock on
    private bool isDisabled = false;        // Checks if lock on is disabled

    // Use this for initialization
    void Awake()
    {
        current = this;

        reticleAnimator = lockOnReticle.GetComponent<Animator>();
        defaultRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        GetLockOn();
        AdjustLockOnReticle();
        ChangeRotation();
        CheckForDeath();
    }

    // Checks for a doubletap
    void FixedUpdate()
    {
        if (disableLockOn > 0.0f)
        {
            disableLockOn -= Time.deltaTime;
        }
    }

    // Checks for a doubletap
    void DisableLockOn()
    {
        if (Input.GetButtonDown("LockOn"))
        {
            // If pressed fast enough, disable lock on
            if (disableLockOn > 0.0f)
            {
                nearestEnemy = null;
                disableLockOn = 0.0f;
                isDisabled = true;

                reticleAnimator.SetBool("Exit", true);
                Invoke("DisableExitAnim", 0.3f);
                return;
            }
            disableLockOn = 0.2f;
            isDisabled = false;
            return;
        }
    }

    // Acquires a lock on if there is a suitable target
    public void GetLockOn()
    {
        float nearestDistance = Mathf.Infinity;

        DisableLockOn();

        // If lock on is not disabled and there is a valid target, lock on
        if ((shouldLockOn == true && isDisabled == false) || Input.GetButtonDown("LockOn"))
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemies"))
            {
                float distance = (transform.position - obj.transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    if (disableLockOn > 0.0f && lockOnReticle.GetComponent<SpriteRenderer>().color.a < 0.534f)
                    {
                        reticleAnimator.SetBool("Intro", true);
                        Invoke("DisableIntroAnim", 0.3f);
                    }
                    nearestDistance = distance;
                    nearestEnemy = obj;
                    shouldLockOn = false;
                }
            }
            LockOnSound();

            previousEnemy = nearestEnemy;
        }
    }

    void LockOnSound()
    {
        // If locked on and not locking on to the same enemy, adjust the lock on reticle
        if (nearestEnemy != null && nearestEnemy.transform.parent == true)
        {
            // Plays audio
            if (nearestEnemy != previousEnemy || lockOnReticle.GetComponent<SpriteRenderer>().color.a < 0.528f)
            {
                if (lockOnReticle.GetComponent<AudioSource>() != null)
                {
                    lockOnReticle.GetComponent<AudioSource>().Play();
                }
            }
        }
        else
        {
            shouldLockOn = true;
        }
    }

    void AdjustLockOnReticle()
    {
        if (nearestEnemy != null)
        {
            lockOnReticle.transform.position = nearestEnemy.transform.position;
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

    // If doing an action, change the rotation of the actor
    public void ChangeRotation()
    {
        // Checks if player is to the right of an enemy
        if (nearestEnemy != null)
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

        // Checks for a melee attack
        if (PlayerManager.current.isMeleeAttacking == false)
        {
            // Checks if shooting a weapon
            if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
            {
                if (nearestEnemy != null && isDisabled == false)
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
            else
            {   // Otherwise go to the default rotation
                transform.rotation = defaultRotation;

                if (!facingRight)
                {
                    transform.rotation *= Quaternion.Euler(0, 180f, 0);
                }
            }
        }
        else if (PlayerManager.current.isMeleeAttacking && isDisabled == false)
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

        if (nearestEnemy == null || isDisabled == true)
        {
            transform.rotation = defaultRotation;
        }
    }

    // If the game object is no longer active and currently searching for a lock on, switch
    void CheckForDeath()
    {
        if (shouldLockOn == true && isDisabled == false)
        {
            int i = 0;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemies"))
            {
                i++;
            }

            if (i == 0)
            {
                reticleAnimator.SetBool("Exit", true);
                Invoke("DisableExitAnim", 0.3f);
                isDisabled = true;
                nearestEnemy = null;
            }
        }
    }
}
