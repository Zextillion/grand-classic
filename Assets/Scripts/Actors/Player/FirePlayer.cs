﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FirePlayer : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;                // The bullet prefab to fire
    private bool willGrow = true;           // Set true if you want to expand the pool if there are too many instances of the object
    private string bulletName;              // The name of the bullet prefab
    [SerializeField]
    GameObject muzzleFlash;                 // Muzzle flash prefab
    private string muzzleFlashName;

    [SerializeField]
    float startUpDelay = 0.0f;              // How long to wait until actually shooting
    [SerializeField]
    float fireTime = 0.1f;                  // How fast the gun shoots
    private bool cooldown = true;           // Internal calculation for if a weapon can shoot - true means ready to shoot

    [SerializeField]
    bool singleShot = false;                // Determines if gun is a single-shot burst
    private bool hasShot = false;           // Determines if gun has fired

    [SerializeField]
    int spreadAngle = 10;                   // How big the spread should be
    private Quaternion initialAngle;        // The inital angle of the weapon
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

    [SerializeField]
    string fireButton = "Fire1";

    private Rigidbody2D rb;

    void Awake()
    {
        bulletName = bulletPrefab.transform.name;
        muzzleFlashName = muzzleFlash.transform.name;
        initialAngle = transform.localRotation;
    }

    void Start()
    {
        rb = PlayerManager.current.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((singleShot == false || (singleShot == true && hasShot == false))
        && PlayerManager.current.canAct == true
        && PlayerManager.current.isDashing == false
        && cooldown == true)
        {
            CheckForInput();
        }
        CheckForMelee();
        CheckForLift();
    }

    void CheckForInput()
    {
        if (Input.GetButton(fireButton) && (PlayerManager.current.fireButton == fireButton || PlayerManager.current.fireButton == ""))
        {
            PlayerManager.current.fireButton = fireButton;
            cooldown = false;
            StartUpDelay();
        }
    }

    // Checks if player no longer pressing input
    void CheckForLift()
    {
        if (Input.GetButton(fireButton) == false)
        {
            // Reinitializes the spread
            transform.localRotation = initialAngle;
            hasShot = false;
        }
    }

    #region Fire
    void StartUpDelay()
    {
        PlayerManager.current.IsShooting(startUpDelay + cancelTime);
        if (allowMovement == false)
        {
            DisableActing();
        }
        if (hasShot == false)
        {
            hasShot = true;
            Invoke("Fire", startUpDelay);
        }
        else
        {
            Fire();
        }
    }

    void DisableActing()
    {
        PlayerMovement.current.rb.velocity = Vector2.zero;
        PlayerManager.current.canMove = false;
    }

    // Main fire function
    void Fire()
    {
        // Do not fire bullets that are already active
        // Gets an object from the pool
        LockOn.current.ChangeRotation();
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
        PlayAudio();
        MuzzleFlash();
        // Knockback
        rb.AddForce(transform.right * knockbackAmount * 1000 * -1);
        // Spread
        if (singleShot == false)
        {
            transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, initialAngle.z + Random.Range(spreadAngle, -spreadAngle));
        }
        // Screen shake
        CameraShake.current.Shake(shakeDuration, shakeAmount, decreaseFactor);
        Invoke("ResetFireTime", fireTime);
        Invoke("CanAct", cancelTime);
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
    #endregion

    #region End Attack
    // Allows the gun to shoot again
    void ResetFireTime()
    {
        cooldown = true;
    }

    // Allows the player to do other actions again
    void CanAct()
    {        
        PlayerManager.current.canAct = true;
        PlayerManager.current.canMove = true;
    }
    #endregion

    // Ensures that melee attack cancels correctly and is not overwritten by functions in this class
    void CheckForMelee()
    {
        if (PlayerManager.current.isMeleeAttacking == true)
        {
            //CancelInvoke();
        }
    }
}
