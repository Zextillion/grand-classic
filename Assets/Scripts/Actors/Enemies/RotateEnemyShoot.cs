using UnityEngine;
using System.Collections;

public class RotateEnemyShoot : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;              // The bullet prefab
    [SerializeField]
    float cooldown = 0.0f;          // The cooldown in between shots
    private string objectType;      // The name of the bullet
    private bool willGrow = true;   // Set true if you want to expand the pool if there are too many instances of the object

    [SerializeField]
    bool oneShot = false;           // Determines if the attack is continuous or not
    private bool hasShot = false;   // If only one attack, determines if already attacked

    private bool readyToShoot = true;
	
    void Awake()
    {
        objectType = bullet.transform.name;
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
                Fire();
                Invoke("Cooldown", cooldown);
                readyToShoot = false;
            }
        } 
	}

    // Fires a shot or two
    void Fire()
    {
        // Gets an object from the pool
        GameObject obj = ObjectPooler.current.GetObjectForType(objectType, willGrow);

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
