using UnityEngine;
using System.Collections;

// Player movement script
// IMPORTANT: Required game objects: Player manager
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement current;

    public float speed = 10.0f;
    [HideInInspector]
    public float initialSpeed;
    [SerializeField]
    float dashSpeed = 30.0f;
    [HideInInspector]
    public bool cancelledShooting = false;      // Check for if player was shooting at the time

    [SerializeField]
    float lungeTime = 0.1f;                     // Initial dash timer
    [HideInInspector]
    public Rigidbody2D rb;

    private Vector2 lungeDirection;             // Direction of the lunge

    // Initializes initial speed
    void Awake()
    {
        current = this;
        initialSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (PlayerManager.current.isMeleeAttacking == false && PlayerManager.current.canMove == true)
        {
            BasicMovement();
        }
        // Allows dashing continuously by spamming the dash button
        if (PlayerManager.current.isDashing == true)
        {
            Dash();
            Lunge();
        }
    }

    // Basic 2D movement
    public void BasicMovement()
    {
        // Basic movement code 
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * 100 * Time.deltaTime;
       
        Dash();
    }

    void Dash()
    {
        // Input for dashing
        if (Input.GetButtonDown("Dash") && PlayerManager.current.canAct)
        {
            lungeDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            CancelShooting();
            Lunge();
            CancelInvoke();
        }
    }

    void Lunge()
    {
        Invoke("EndLunge", lungeTime);
        PlayerManager.current.isDashing = true;
        PlayerManager.current.canMove = false;
        rb.velocity = lungeDirection * dashSpeed;
    }

    void CancelShooting()
    {
        if (PlayerManager.current.isShooting == true)
        {
            PlayerManager.current.readyToShoot = false;
            PlayerManager.current.isShooting = false;
            cancelledShooting = true;
        }
    }

    void EndLunge()
    {
        CancelInvoke();
        speed = initialSpeed;
        PlayerManager.current.isDashing = false;
        PlayerManager.current.canMove = true;

        // If cancelled shooting, reenable the ability to shoot
        if (cancelledShooting == true)
        {
            PlayerManager.current.readyToShoot = true;
            cancelledShooting = false;
        }
    }
}
