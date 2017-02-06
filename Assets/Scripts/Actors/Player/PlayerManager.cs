using UnityEngine;
using System.Collections;

// Player manager
// Manages movement, if the player can shoot, etc.
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;
    [HideInInspector]
    public bool readyToShoot = true;        // Check if player can shoot (cooldown over)
    [HideInInspector]
    public bool isDashing = false;          // Checks for the initial "lunge", NOT if it's still dashing
    [HideInInspector]
    public bool isMeleeAttacking = false;   // Checks if player is in middle of melee attack
    [HideInInspector]
    public bool isShooting = false;         // Checks if player is shooting gun
    [HideInInspector]
    public bool canAct = true;              // Kept separate from other state checks for "animation cancelling"
    [HideInInspector]
    public bool canMove = true;             // Checks if player can move
    [HideInInspector]
    public Animator bodyAnimator;           // Animator for the main sprite
    [HideInInspector]
    public string fireButton;

    public float speed = 1.0f;
    public float dashSpeed = 2.0f;
    public float lungeTime = 0.1f;

    // Checks for cancelling attacks into dashes and vice versa
    [HideInInspector]
    public bool continuousAttacking = false;

    void Awake()
    {
        current = this;
        bodyAnimator = transform.Find("Sprite").GetComponent<Animator>();
    }

    // Disables isShooting. Cancel time is cancel time with a short buffer.
    public void IsShooting(float cancelTime)
    {
        CancelInvoke();
        cancelTime += 0.1f;
        isShooting = true;
        Invoke("DisableIsShooting", cancelTime);
    }

    void DisableIsShooting()
    {
        isShooting = false;
        fireButton = "";
    }
}
