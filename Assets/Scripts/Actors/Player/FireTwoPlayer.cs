using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Generally same as FireOnePlayer, except using another input button
public class FireTwoPlayer : MonoBehaviour
{
    public static FireTwoPlayer current;
    [SerializeField]
    GameObject bulletPrefab;                // The bullet prefab to fire
    private bool willGrow = true;           // Set true if you want to expand the pool if there are too many instances of the object
    private string bulletName;              // The name of the bullet prefab
    [SerializeField]
    GameObject muzzleFlash;                 // Muzzle flash prefab
    private string muzzleFlashName;

    [SerializeField]
    float fireTime = 0.1f;                  // How fast the gun shoots
    [HideInInspector]
    public bool canFire = true;             // Checks if cooldown is over
    [HideInInspector]
    public bool isShooting = false;         // Cancels other input

    [SerializeField]
    bool singleShot = false;                // Determines if gun is a single-shot burst
    private bool hasShot = false;           // Determines if gun has fired
    [SerializeField]
    int spreadAngle = 10;                   // How big the spread should be
    [SerializeField]
    float knockbackAmount = 1.0f;           // How far the gun should knockback the user

    [SerializeField]
    float cancelTime = 0.05f;               // When the player can act again after firing a bullet
    [SerializeField]
    bool allowMovement = true;              // If true, allow the player to move when attacking

    [SerializeField]
    float shakeDuration = 0.1f;             // How long the screen should shake.     
    [SerializeField]
    float shakeAmount = 1.0f;               // How hard the screen should shake
    [SerializeField]
    float decreaseFactor = 1.0f;            // How much the shake should decrease

    void Awake()
    {
        current = this;
        bulletName = bulletPrefab.transform.name;
        muzzleFlashName = muzzleFlash.transform.name;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForOtherFire();
        CheckForLift();
        if (canFire == true)
        {
            if (singleShot == false || (singleShot == true && hasShot == false))
            {
                CheckForInput();
            }
        }
        CheckForMelee();
    }

    void CheckForInput()
    {
        if (Input.GetButton("Fire2")
        && PlayerManager.current.canAct == true
        && PlayerManager.current.isMeleeAttacking == false
        && PlayerMovement.current.cancelledShooting == false)
        {
            PlayerManager.current.isShooting = true;

            if (PlayerManager.current.readyToShoot
                && PlayerManager.current.isDashing == false)
            {   // If the gun is not on cooldown
                LockOn.current.ChangeRotation();
                Fire();
                isShooting = true;

                // How fast the gun can fire
                PlayerManager.current.readyToShoot = false;
                hasShot = true;
                Invoke("ResetFireTime", fireTime);
                Invoke("CanAct", cancelTime);
            }
        }
    }

    void CheckForOtherFire()
    {
        if (FireOnePlayer.current.isShooting == true)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                FireOnePlayer.current.canFire = false;
            }
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            FireOnePlayer.current.canFire = true;
        }
    }

    // Checks if player no longer pressing input
    void CheckForLift()
    {
        if (Input.GetButtonUp("Fire2"))
        {
            // Reinitializes the spread
            transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
            PlayerManager.current.isShooting = false;
            hasShot = false;
        }
    }

    #region Fire
    // Main fire function
    void Fire()
    {
        // Do not fire bullets that are already active
        // Gets an object from the pool
        GameObject obj = ObjectPooler.current.GetObjectForType(bulletName, willGrow);
        if (obj == null)
        {
            return;
        }
        else
        {   // else fire a bullet from the position of the barrel
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
        }

        AllowMovement();

        PlayAudio();

        MuzzleFlash();

        Knockback();

        Spread();

        Screenshake();
    }

    void AllowMovement()
    {
        if (allowMovement == false)
        {
            PlayerManager.current.canMove = false;
        }
    }

    void PlayAudio()
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }

    }

    void MuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            GameObject obj = ObjectPooler.current.GetObjectForType(muzzleFlashName, willGrow);
            if (obj == null)
            {
                return;
            }
            else
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
            }
        }
    }

    void Knockback()
    {
        Rigidbody2D rb = PlayerManager.current.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * knockbackAmount * 1000 * -1);   
    }

    void Spread()
    {
        transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(spreadAngle, -spreadAngle));
    }

    void Screenshake()
    {
        CameraShake.current.Shake(shakeDuration, shakeAmount, decreaseFactor);
    }
    #endregion

    #region End Attack
    // Allows the gun to shoot again
    void ResetFireTime()
    {
        PlayerManager.current.readyToShoot = true;
    }

    // Allows the player to do other actions again
    void CanAct()
    {
        PlayerManager.current.canAct = true;
        PlayerManager.current.canMove = true;
        isShooting = false;
    }
    #endregion

    // Ensures that melee attack cancels correctly and is not overwritten by functions in this class
    void CheckForMelee()
    {
        if (PlayerManager.current.isMeleeAttacking == true)
        {
            CancelInvoke();
        }
    }
}
