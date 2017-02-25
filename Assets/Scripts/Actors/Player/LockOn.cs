using UnityEngine;
using System.Collections;

// LockOn class for the player
public class LockOn : MonoBehaviour
{
    public static LockOn current;
    private GameObject lockOnReticle;               // The reticle used for determining if there is a lock on
    private Animator reticleAnimator;       // The animator for the reticle
    private GameObject cameraTarget;

    private int enemyList;
    private Quaternion defaultRotation;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public GameObject nearestEnemy = null;
    private GameObject previousEnemy = null;

    [HideInInspector]
    public bool shouldLockOn = true;       // If there is no lock on and a target enters the eligible range, lock on

    private bool disableLockOn = false;     // Countdown to disable lock on
    private bool isDisabled = false;        // Checks if lock on is disabled

    // Use this for initialization
    void Awake()
    {
        current = this;

        lockOnReticle = GameObject.Find("LockOnReticle");
        reticleAnimator = lockOnReticle.GetComponent<Animator>();
        cameraTarget = GameObject.Find("CameraTarget");
        defaultRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // If lock on is not disabled and there is a valid target, lock on
        if ((shouldLockOn == true && isDisabled == false) || Input.GetButtonDown("LockOn"))
        {
            GetLockOn();
        }
        if (nearestEnemy != null)
        {
            AdjustLockOnReticle();
            CheckFacingRight();
            ChangeRotation();
            CheckForDeath();
        }
        AdjustCameraTarget();
    }

    // Checks for a doubletap
    void DisableLockOn()
    {
        if (Input.GetButtonDown("LockOn"))
        {
            // If pressed fast enough, disable lock on
            if (disableLockOn == true)
            {
                nearestEnemy = null;
                disableLockOn = false;
                isDisabled = true;

                reticleAnimator.SetBool("Exit", true);
                Invoke("DisableExitAnim", 0.3f);
                return;
            }
            disableLockOn = true;
            Invoke("DoubleTapLockOn", 0.2f);
            isDisabled = false;
            return;
        }
    }

    void DoubleTapLockOn()
    {
        disableLockOn = false;
    }

    // Acquires a lock on if there is a suitable target
    public void GetLockOn()
    {
        float nearestDistance = Mathf.Infinity;

        DisableLockOn();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemies"))
        {
            float distance = (transform.position - obj.transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                if (disableLockOn == true && lockOnReticle.GetComponent<SpriteRenderer>().color.a < 0.534f)
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

    // Play lock on sound
    void LockOnSound()
    {
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

    // Updates the cursor's position each frame
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
        // Checks if attacking
        if (PlayerManager.current.isShooting == true || PlayerManager.current.isMeleeAttacking == true || PlayerManager.current.fireButton != "")
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


        if (nearestEnemy == null || isDisabled == true)
        {
            transform.rotation = defaultRotation;
        }
    }

    public void CheckFacingRight()
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
            PlayerMovement.current.MoveAnimation();
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

    void AdjustCameraTarget()
    {
        if (nearestEnemy != null)
        {
            float distanceX = (nearestEnemy.transform.position.x - transform.position.x);
            float distanceY = (nearestEnemy.transform.position.y - transform.position.y);
            float distance = Vector3.Distance(nearestEnemy.transform.position, transform.position);
            float newPosX = transform.position.x + distanceX / 2;
            float newPosY = transform.position.y + distanceY / 2;
            cameraTarget.transform.position = new Vector3(newPosX, newPosY, 0);
            Camera.main.orthographicSize = 5 + (distance / 3);
        }
        else
        {
            cameraTarget.transform.position = transform.position;
            Camera.main.orthographicSize = 10;
        }
    }
}
