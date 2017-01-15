using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the weapons that Gabi uses
// Weapon List:
/*
 * Weapon 0: Bullet wall
 * Weapon 1: Circle attack (expands)
 * Weapon 2: Bulelt wall that speeds up
 * Weapon 3: Bullet "bubble"
 */
public class WeaponsGabi : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks = 0.5f;        // The time in between attacks
    [SerializeField]
    float marginTime = 0.0f;                // The margin of error
    private float randomTime = 0.0f;

    [SerializeField]
    GameObject[] weapons;                   // The array of weapons
    private GameObject nextAttack;          // Her next attack
    private GameObject previousAttack;      // Her last used attack

    private float disableWeapon = 0.2f;     // How long the attack lasts
    private float cooldown = 0.1f;          // The cooldown between her attacks
    private bool readyToAttack = true;      // If she is ready to attack

    private NoRotationEnemyLockOn lockOn;

    private int numAttacks = 0;
    private bool rotateCircle = false;      // Checks if one of these shield things are up

    private Health bossHealth;              // Used for checking the percent of remaining hp
    [SerializeField]
    GameObject[] ultimateAttacks = new GameObject[3];   
    private bool[] ultimateAttacksCheck = new bool[3];  // Used to determine which stage of the boss it's on

    void Awake()
    {
        lockOn = gameObject.GetComponent<NoRotationEnemyLockOn>();
        bossHealth = GetComponentInChildren<Health>();
        for (int i = 0; i < ultimateAttacksCheck.Length; i++)
        {
            ultimateAttacksCheck[i] = false;
        }
    }
	// Use this for initialization
	void Start ()
    {
        nextAttack = weapons[0];    // Bullet wall will always be the first attack
        RandomTime();
        InvokeRepeating("EnableWeapon", 2.0f, timeBetweenAttacks + randomTime);
    }

    void Update()
    {
        ChangeRotation();
    }

    void RandomTime()
    {
        randomTime = Mathf.Floor(Random.value * 0.5f);
    }

    // Enables the next weapon
    void EnableWeapon()
    {
        if (readyToAttack)
        {
            nextAttack.SetActive(true);
            readyToAttack = false;
            previousAttack = nextAttack;
            numAttacks++;
            ChangeAttack();
            Invoke("DisableWeapon", disableWeapon);
        }
    }

    // Changes the attack
    void ChangeAttack()
    {
        float healthPercent;
        healthPercent = bossHealth.CalculateHealthPercent();
        Debug.Log("HP: " + healthPercent);
        RandomTime();
        // First ultimate at 75% hp
        if (healthPercent <= 75.0f && ultimateAttacksCheck[0] == false)
        {
            CancelInvoke();
            Debug.Log("Cancelinvoke1");
            Invoke("UltimateOne", timeBetweenAttacks + randomTime);
        }
        else if (healthPercent <= 50.0f && ultimateAttacksCheck[1] == false)
        {   // Second at 50%
            CancelInvoke();

            Debug.Log("Cancelinvoke2");
            nextAttack = ultimateAttacks[1];
            Invoke("UltimateTwo", timeBetweenAttacks + randomTime);
        }
        else if (healthPercent <= 25.0f && ultimateAttacksCheck[2] == false)
        {   // Last at 25%
            CancelInvoke();
            Debug.Log("Cancelinvoke3");
            nextAttack = ultimateAttacks[2];
            Invoke("UltimateThree", timeBetweenAttacks + randomTime);
        }
        else if (healthPercent <= 0.0f)
        {
            CancelInvoke();
        }
        else
        {
            if (ultimateAttacksCheck[2] == true)
            {
                Debug.Log("Stage four");
                StageOne();
            }
            else if (ultimateAttacksCheck[1] == true)
            {
                StageOne();
                Debug.Log("Stage three");
            }
            else if (ultimateAttacksCheck[0] == true)
            {
                Debug.Log("Stage two");
                StageOne();
            }
            else
            {
                Debug.Log("Stage one");
                StageOne();
            }
        }
    }

    // First ultimate
    void UltimateOne()
    {
       // nextAttack = ultimateAttacks[0];
        Debug.Log("Ultimate one!");
        ultimateAttacksCheck[0] = true;

        // Swap weapons to stage two
        StageOne(); //FIXME: Change to stage two
        InvokeRepeating("EnableWeapon", 2.0f, timeBetweenAttacks + randomTime);
    }

    // Second ultimate
    void UltimateTwo()
    {
     //   nextAttack = ultimateAttacks[1];
        Debug.Log("Ultimate two!");
        ultimateAttacksCheck[1] = true;

        // Swap weapons to stage three
        StageOne(); //FIXME: Change to stage 3
        InvokeRepeating("EnableWeapon", 2.0f, timeBetweenAttacks + randomTime);
    }

    // Third ultimate
    void UltimateThree()
    {
     //   nextAttack = ultimateAttacks[2];
        Debug.Log("Ultimate three!");
        ultimateAttacksCheck[2] = true;

        // Swap weapons to stage four
        StageOne(); //FIXME: Change to stage 4
        InvokeRepeating("EnableWeapon", 2.0f, timeBetweenAttacks + randomTime);
    }

    // Attacks if no ultimates have played
    void StageOne()
    {
        if (numAttacks < 5)
        {
            int randomValue;
            randomValue = (int)Mathf.Floor(Random.value * 100);
            if (randomValue >= 0 && randomValue < 50)
            {   // 50% chance
                BulletWall();               // Bullet wall attack
            }
            else if (randomValue >= 50 && randomValue < 100)
            {
                CircleAttack();             // Circle attack
            }
            else
            {
                nextAttack = weapons[0];    // Default weapon: bullet wall
            }
        }
        else
        {
            DeployDrone();
            numAttacks = 0;
        }
    }

    // Changes next weapon to a bullet wall attack
    void BulletWall()
    {
        int randomValue;
        randomValue = (int)Mathf.Floor(Random.value * 100);
        if (randomValue >= 0 && randomValue < 50)
        {   // 50% chance
            nextAttack = weapons[0];    // slow bullet wall
        }
        else if (randomValue >= 50 && randomValue < 100)
        {
            nextAttack = weapons[2];    // Faster bullet wall
        }
        else
        {
            nextAttack = weapons[0];    // Default weapon: slow bullet wall
        }
    }

    // Changes next weapon to a circle attack
    void CircleAttack()
    {
        int randomValue;
        randomValue = (int)Mathf.Floor(Random.value * 100);
        if (randomValue >= 0 && randomValue < 50)
        {   // 50% chance
            nextAttack = weapons[1];    // Circular attack
        }
        else if (randomValue >= 50 && randomValue < 100)
        {
            if (rotateCircle == true)
            {
                ChangeAttack();
            }
            else
            {
                CircleRotateAttack();
            }
        }
        else
        {
            nextAttack = weapons[1];    // Default weapon: regular circle attack
        }
    }

    void CircleRotateAttack()
    {
        nextAttack = weapons[3];    // Circular rotateattack
        rotateCircle = true;
        Invoke("DisableCircleRotateAttack", 5.0f);   // Disables with about how long the attack lasts
    }
    void DisableCircleRotateAttack()
    {
        rotateCircle = false;
    }

    void DeployDrone()
    {
        nextAttack = weapons[4];    // Deploys a drone
    }
















    // Disables the weapon
    void DisableWeapon()
    {
        previousAttack.SetActive(false);
        Invoke("ResetReadyToAttack", cooldown);
    }

    // Resets readyToAttack
    void ResetReadyToAttack()
    {
        readyToAttack = true;
    }

    // Changes rotation to face lock on target
    void ChangeRotation()
    {
        // Checks if there is a lock on
        if (lockOn.nearestEnemy != null && readyToAttack == false)
        {
            Vector3 vectorToTarget = lockOn.nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 999999);

            if (!lockOn.facingRight)
            {
               transform.rotation *= Quaternion.Euler(180f, 0, 0);
            }
        }
        else
        {
            if (transform.position.x > lockOn.nearestEnemy.transform.position.x)
            {
                transform.rotation = lockOn.defaultRotation;
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                transform.rotation = lockOn.defaultRotation;   
            }
        }
    }
}
