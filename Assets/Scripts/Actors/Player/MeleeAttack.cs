using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Activates the hitbox for melee weapons
public class MeleeAttack : MonoBehaviour
{
    RequireComponent BoxCollider2D;         // This script needs a Box Collider 2D in order to work

    [SerializeField]
    float hitboxDuration = 0.1f;            // Determines how long the hitbox stays up
    [SerializeField]
    float successCancelTime = 0.05f;        // The cancel time if successfully hits a target
    [SerializeField]
    float cancelTime = 0.1f;                // When the player can act again after attacking if attack does not connect
    [SerializeField]
    float lungeSpeed = 30.0f;               // The speed the player should lunge
    [SerializeField]
    float lungeTimer = 2.5f;                // How long the player should lunge
    private float lungeCounter;             // Counts up to lungeTimer
    private bool successfulHit = false;     // Determines if hit an enemy

    [SerializeField]
    float shakeDuration = 0.1f;             // How long the screen should shake.     
    [SerializeField]
    float shakeAmount = 0.1f;               // How hard the screen should shake
    [SerializeField]
    float decreaseFactor = 1.0f;            // How much the shake should decrease

    private BoxCollider2D col;              // Hitbox
    private Transform parent;               // Parent of what should be the hitbox and sprite of the melee weapon
    private SpriteRenderer sprite;          // Sprite of the melee weapon

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        parent = gameObject.transform.parent;
        sprite = parent.GetComponentInChildren<SpriteRenderer>();

        lungeCounter = lungeTimer + 1;
    }

	// Update is called once per frame
	void Update()
    {
		if (Input.GetButtonDown("Fire3") && PlayerManager.current.isMeleeAttacking == false && PlayerManager.current.canAct)
        {
            if (PlayerManager.current.isDashing == false)
            {   // If not on cooldown
                Attack();
            }
        }
    }

    // Enables the hitbox and sprite
    void Attack()
    {
        col.enabled = true;     // Enables hitbox
        sprite.enabled = true;  // Enables sprite

        PlayerManager.current.isMeleeAttacking = true;

        // Lunge
        lungeCounter = 0.0f;

        // Leaves the hitbox up for the hitboxDuration
        CancelInvoke();
        Invoke("DisableHitbox", hitboxDuration);

        // How fast the player can act
        PlayerManager.current.readyToShoot = false;
        PlayerManager.current.canAct = false;
        Invoke("DisableSprite", cancelTime);
        Invoke("CanAct", cancelTime);

        // Screenshake
        Screenshake();
    }

    void Screenshake()
    {
        CameraShake.current.Shake(shakeDuration, shakeAmount, decreaseFactor);
    }

    // Checks for a hit, determine if can cancel early
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Enemies")
        {
            successfulHit = true;
            CancelInvoke();
            Invoke("DisableHitbox", hitboxDuration);
            Invoke("DisableSprite", hitboxDuration);
            Invoke("CanAct", successCancelTime);
        }
    }

    void DisableHitbox()
    {
        col.enabled = false;
    }

    void DisableSprite()
    {
        sprite.enabled = false;
    }

    void CanAct()
    {
        PlayerManager.current.isMeleeAttacking = false;
        PlayerManager.current.readyToShoot = true;
        PlayerManager.current.canAct = true;
        PlayerManager.current.canMove = true;
        successfulHit = false;
    }

    // Calls camera shake and lunge
    void FixedUpdate()
    {
        // Lunge
        if (lungeCounter < lungeTimer)
        {   // Lunge
            PlayerMovement.current.rb.velocity = transform.right * lungeSpeed * 100 * Time.deltaTime;
            lungeCounter += Time.deltaTime;
        }
        else if (lungeCounter >= lungeTimer)
        {   // Player is done lunging, but can not act
            PlayerMovement.current.rb.velocity = new Vector3(0, 0, 0);
        }
        else if (PlayerManager.current.canAct == true)
        {   // Player can act now
            if (Input.GetButtonDown("Dash"))
            {
                PlayerManager.current.isMeleeAttacking = false;
            }
            if (PlayerManager.current.canAct == true && PlayerManager.current.readyToShoot == true)
            {
                PlayerMovement.current.BasicMovement();
            }
        }
    }
}
