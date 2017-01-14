using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;              // The bullet prefab
    private string objectType;      // The name of the bullet
    private bool willGrow = true;   // Set true if you want to expand the pool if there are too many instances of the object
    [SerializeField]
    float startDelay = 0.0f;        // The time it takes for the first shots to be fired

    [SerializeField]
    float cooldown = 0.0f;          // The cooldown in between bursts
    [SerializeField]
    int burstNumber = 1;            // The number of bullets to fire in a burst
    [SerializeField]
    float timeBetweenShots = 0.0f;  // Time between individual shots in one burst
    [SerializeField]
    float spreadAngle = 0.0f;       // Angle of spread
    private Quaternion initialRotation;
    private int shotsFired = 0;     // Shots fired in a single burst

    [SerializeField]
    bool oneShot = false;           // Determines if the attack is continuous or not
    private bool hasShot = false;   // If only one attack, determines if already attacked

    private bool readyToShoot = true;

    void Awake()
    {
        objectType = bullet.transform.name;
        initialRotation = transform.localRotation;
    }

    void OnEnable()
    {
        readyToShoot = false;
        Invoke("Cooldown", startDelay);
    }

	// Update is called once per frame
	void Update ()
    {
        // Checks if attack is single burst
        if (oneShot == false || (oneShot == true && hasShot == false))
        {
            // If the gun is not on cooldown
            if (readyToShoot)
            {
                Burst();  
            }
        } 
	}

    // Checks initial burst
    void Burst()
    {
        if (readyToShoot == true)
        {
            if (shotsFired < burstNumber)
            {
                Fire();
                Spread();
                shotsFired++;
                Invoke("Cooldown", timeBetweenShots);
                readyToShoot = false;
            }
            else
            {
                readyToShoot = false;
                CancelInvoke();
                Invoke("Cooldown", cooldown);
                shotsFired = 0;
                ResetSpread();
            }
        }
    }

    // Fires a shot or two
    void Fire()
    {
        // Gets an object from the pool
        GameObject obj = null;
        obj = ObjectPooler.current.GetObjectForType(objectType, willGrow);

        // If there is no bullet to fire, don't fire a bullet
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
        hasShot = true;
    }

    void Spread()
    {
        transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, initialRotation.z + Random.Range(spreadAngle, -spreadAngle));
    }

    void ResetSpread()
    {
        // Reinitializes the spread
        transform.localRotation = initialRotation;
    }

    // Cooldown before the next fire cycle
    void Cooldown()
    {
        readyToShoot = true;
    }

    // On disable, reset hasShot
    void OnDisable()
    {
        hasShot = false;
    }
}
