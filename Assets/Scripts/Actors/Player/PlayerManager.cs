using UnityEngine;
using System.Collections;

// Player manager
// Manages movement, if the player can shoot, etc.
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;
    [HideInInspector]
    public bool readyToShoot = true;        // Check if player can shoot
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
    public bool canRotate = false;          // Checks if player can rotate (to lock on)

    // Checks for cancelling attacks into dashes and vice versa
    [HideInInspector]
    public bool continuousAttacking = false;

    void Awake()
    {
        current = this;
    }
}
