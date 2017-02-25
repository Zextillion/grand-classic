using UnityEngine;
using System.Collections;

// Player movement script
// IMPORTANT: Required game objects: Player manager
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement current;

    float speed = 10.0f;
    float dashSpeed = 30.0f;
    float lungeTime = 0.1f;                     // Initial dash timer
    private Vector2 lungeDirection;

    [HideInInspector]
    public Rigidbody2D rb;
    
    private GameObject hitbox;

    void Awake()
    {
        current = this;
        rb = GetComponent<Rigidbody2D>();
        hitbox = transform.Find("Hitbox").gameObject;
    }

    void Start()
    {
        speed = PlayerManager.current.speed;
        dashSpeed = PlayerManager.current.dashSpeed;
        lungeTime = PlayerManager.current.lungeTime;
    }

    void Update()
    {
        if (PlayerManager.current.isMeleeAttacking == false && PlayerManager.current.isDashing == false && PlayerManager.current.canMove == true)
        {
            BasicMovement();
        }

        if (Input.GetButtonDown("Dash") && (PlayerManager.current.canAct || PlayerManager.current.canMove) && PlayerManager.current.isShooting == false)
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
    }

    // Turns player around depending on direction travelling relative to 
    public void MoveAnimation()
    {
        if (((rb.velocity.x < 0) != LockOn.current.facingRight) || rb.velocity.x == 0 )
        {
            PlayerManager.current.bodyAnimator.SetBool("movingBack", false);
        }
        else
        {
            PlayerManager.current.bodyAnimator.SetBool("movingBack", true);
        }
    }

    void Dash()
    {
        CancelInvoke();
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
}
