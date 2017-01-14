using UnityEngine;
using System.Collections;

// Speeds up movement for the bullet after a set amount of time
public class SpeedUpMove : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    float speed = 10.0f;            // The current speed to modify the velocity by
    [SerializeField]
    float timeToSpeedUp = 1.0f;     // The time the bullet is active until it starts speeding up
    [SerializeField]
    float speedIncrement = 1.0f;    // How fast the speed should get faster
    private bool speedUp;           // Determines if the velocity should get faster

    private float defaultSpeed;     // The speed set in the inspector

    // Initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultSpeed = speed;
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (gameObject.activeInHierarchy)
        {
            rb.AddForce(transform.right * speed * 100 * Time.deltaTime);
        }
        if (speedUp == true)
        {
            speed += speedIncrement;
        }
    }

    // Starts speeding up the game object
    void SpeedUp()
    {
        speedUp = true;
    }

    // Resets speed
    void OnEnable()
    {
        rb.velocity = Vector3.zero;
        speed = defaultSpeed;
        Invoke("SpeedUp", timeToSpeedUp);
    }

    // Removes speedUp speed
    void OnDisable()
    {
        CancelInvoke();
        speedUp = false;
    }
}
