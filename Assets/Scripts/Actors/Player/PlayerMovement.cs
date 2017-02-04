using UnityEngine;
using System.Collections;

// Player movement script
// IMPORTANT: Required game objects: Player manager
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement current;

    float speed = 10.0f;
    float initialSpeed;
    float dashSpeed = 30.0f;
    float lungeTime = 0.1f;                     // Initial dash timer
    private Vector2 lungeDirection;
    private Vector3 previousPosition;

    [HideInInspector]
    public Rigidbody2D rb;
    
    private GameObject hitbox;

    [HideInInspector]
    public bool cancelledShooting = false;      // Check for if player was shooting at the time

    void Awake()
    {
        current = this;
        rb = GetComponent<Rigidbody2D>();
        hitbox = GameObject.Find("Hitbox");
    }

    void Start()
    {
        speed = PlayerManager.current.speed;
        initialSpeed = speed;
        dashSpeed = PlayerManager.current.dashSpeed;
        lungeTime = PlayerManager.current.lungeTime;
        previousPosition = transform.position;
    }

    void Update()
    {
        if (PlayerManager.current.isMeleeAttacking == false && PlayerManager.current.isDashing == false && PlayerManager.current.canMove == true)
        {
            BasicMovement();
        }

        if (Input.GetButtonDown("Dash") && PlayerManager.current.canAct)
        {
            Dash();
        }

        if (PlayerManager.current.isDashing == true)
        {
            Lunge();
        }
    }
    
    // Basic 2D movement
    public void BasicMovement()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * 100 * Time.deltaTime;
        MoveAnimation();
        previousPosition = transform.position;
    }

    // Turns player around depending on direction travelling relative to 
    public void MoveAnimation()
    {
        if (LockOn.current.facingRight == true)
        {
            if (previousPosition.x > transform.position.x)
            {
                PlayerManager.current.bodyAnimator.SetBool("movingBack", true);
            }
            else 
            {
                PlayerManager.current.bodyAnimator.SetBool("movingBack", false);
            }
        }
        else
        {
            if (previousPosition.x < transform.position.x)
            {
                PlayerManager.current.bodyAnimator.SetBool("movingBack", true);
            }
            else
            {
                PlayerManager.current.bodyAnimator.SetBool("movingBack", false);
            }
        }
    }

    void Dash()
    {
        CancelInvoke();
        CancelShooting();
        lungeDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (lungeDirection == Vector2.zero)
        {
            lungeDirection = transform.right;
        }
        hitbox.layer = LayerMask.NameToLayer("BulletPhaser");
        PlayerManager.current.isDashing = true;
        rb.velocity = lungeDirection * dashSpeed;
        Lunge();
    }

    void Lunge()
    {
        Invoke("EndLunge", lungeTime);
        if (Input.GetAxisRaw("Horizontal") != lungeDirection.x)
        {
            rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), 0) * dashSpeed * 12, ForceMode2D.Force);
        }
        if (Input.GetAxisRaw("Vertical") != lungeDirection.y)
        {
            rb.AddForce(new Vector2(0, Input.GetAxisRaw("Vertical")) * dashSpeed * 12, ForceMode2D.Force);
        }
    }

    void EndLunge()
    {
        CancelInvoke();
        PlayerManager.current.isDashing = false;

        Invoke("AllowHit", 0.2f);
    }

    void AllowHit()
    {
        hitbox.layer = LayerMask.NameToLayer("Player");
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
}
