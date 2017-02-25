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
 * Weapon 4: Drone spawner
 * Weapon 5: Missile launcher
 */
public class WeaponsGabi : MonoBehaviour
{
    public static WeaponsGabi current;

    [SerializeField]
    float timeBetweenBursts = 3.0f;         // The time in between bursts
    [SerializeField]
    int numberOfAttacks = 1;                // The amount of attacks in one burst
    [SerializeField]
    float timeBetweenAttacks = 0.5f;        // The time in between attacks
    [SerializeField]
    float marginTime = 0.0f;                // The random time between individual attacks
    private float randomTime = 0.0f;

    [SerializeField]
    GameObject[] weapons;                   // The array of weapons
    private GameObject nextAttack;          // Her next attack
    private GameObject previousAttack;      // Her last used attack

    [SerializeField]
    float moveDelay = 0.5f;                 // How long she should stand still
    private float initialMoveDelay;
    private float disableWeapon = 0.3f;     // How long the attack lasts
    private float cooldown = 0.1f;          // The cooldown between her attacks
    private bool readyToAttack = true;      // If she is ready to attack

    private NoRotationEnemyLockOn lockOn;

    private int numAttacks = 0;
    private int numBursts = 0;
    private bool rotateCircle = false;      // Checks if one of these shield things are up

    [HideInInspector]
    public Health bossHealth;              // Used for checking the percent of remaining hp
    [SerializeField]
    GameObject[] ultimateAttacks = new GameObject[3];
    [SerializeField]
    GameObject[] ultimateDrones = new GameObject[3];
    private bool[] ultimateAttacksCheck = new bool[3];  // Used to determine which stage of the boss it's on
    private bool canRotate = true;

    void Awake()
    {
        lockOn = gameObject.GetComponent<NoRotationEnemyLockOn>();
        initialMoveDelay = moveDelay;
        bossHealth = GetComponentInChildren<Health>();
        current = this;

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
        Invoke("EnableWeapon", 2.0f);
    }

    void Update()
    {
        ChangeRotation();
    }

    void RandomTime()
    {
        randomTime = Mathf.Floor(Random.value * marginTime);
    }

    // Enables the next weapon
    void EnableWeapon()
    {
        GetComponent<FollowObject>().enabled = false;

        nextAttack.SetActive(true);
        readyToAttack = false;
        previousAttack = nextAttack;
        numAttacks++;
        Invoke("DisableWeapon", disableWeapon);
    }

    // Changes the attack
    void ChangeAttack()
    {
        RandomTime();

        UltimateCheck();
    }

    // Checks if an ultimate is ready, if not, do a regular attack
    void UltimateCheck()
    {
        float healthPercent;
        healthPercent = bossHealth.CalculateHealthPercent();
        Debug.Log("HP: " + healthPercent);
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
            StageOne();
        }
    }

    #region Attacks
    void UltimateAttack()
    {
        nextAttack.SetActive(true);

        previousAttack = nextAttack;
    }

    void SecondUltimate()
    {
        previousAttack.SetActive(false);
        canRotate = true;
        readyToAttack = false;
        ChangeRotation();
        canRotate = false;

        if (ultimateAttacksCheck[2] == true)
        {
            nextAttack = ultimateDrones[2];
        }
        else if (ultimateAttacksCheck[1] == true)
        {
            nextAttack = ultimateDrones[1];
        }
        else if (ultimateAttacksCheck[0] == true)
        {
            nextAttack = ultimateDrones[0];
        }
        nextAttack.SetActive(true);
        nextAttack = ultimateAttacks[0];
        Invoke("UltimateAttack", 1.0f);
        Invoke("DisableWeapon", 2.0f);
    }

    // First ultimate
    void UltimateOne()
    {
        ultimateAttacksCheck[0] = true;
        nextAttack = ultimateAttacks[0];

        GetComponent<FollowObject>().enabled = false;
        Invoke("EnableMovement", 5.0f);
        readyToAttack = false;
        canRotate = true;
        ChangeRotation();
        canRotate = false;

        Invoke("UltimateAttack", 1.0f);
        Invoke("DisableUltimate", 2.0f);
    }

    // Second ultimate
    void UltimateTwo()
    {
        ultimateAttacksCheck[1] = true;
        nextAttack = ultimateAttacks[0];

        GetComponent<FollowObject>().enabled = false;
        Invoke("EnableMovement", 5.0f);
        readyToAttack = false;
        canRotate = true;
        ChangeRotation();
        canRotate = false;

        Invoke("UltimateAttack", 1.0f);
        Invoke("DisableUltimate", 2.0f);
    }

    // Third ultimate
    void UltimateThree()
    {
        ultimateAttacksCheck[2] = true;
        nextAttack = ultimateAttacks[0];

        GetComponent<FollowObject>().enabled = false;
        Invoke("EnableMovement", 5.0f);
        readyToAttack = false;
        canRotate = true;
        ChangeRotation();
        canRotate = false;

        Invoke("UltimateAttack", 1.0f);
        Invoke("DisableUltimate", 2.0f);
    }

    // Attacks if no ultimates have played
    void StageOne()
    {
        int i = 0;
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Drone"))
        {
            if (obj.activeInHierarchy == true)
            {
                i++;
            }   
        }

        int randomValue;
        randomValue = (int)Mathf.Floor(Random.value * 100);

        if (lockOn.nearestEnemy != null)
        {
            float distanceToPlayer;
            distanceToPlayer = FindDistanceToObject.current.FindDistance(gameObject, lockOn.nearestEnemy);
            Debug.Log("Distance" + distanceToPlayer);
            if (distanceToPlayer < 13.0f)
            {   // If player is in melee range
                moveDelay = 0.4f;
                MissileLauncher();
            }
            else if (randomValue >= 0 && randomValue < 60)
            {   // 60% chance
                BulletWall();               // Bullet wall attack
            }
            else if (randomValue >= 60 && randomValue < 100)
            {
                CircleAttack();             // Circle attack
            }
            else
            {
                nextAttack = weapons[0];    // Default weapon: bullet wall
            }
        }
    }

    void MissileLauncher()
    {
        nextAttack = weapons[5];    // Missile launcher
    }

    // Changes next weapon to a bullet wall attack
    void BulletWall()
    {
        int randomValue;
        randomValue = (int)Mathf.Floor(Random.value * 100);
        if (randomValue >= 0 && randomValue < 60)
        {   // 60% chance
            nextAttack = weapons[0];    // slow bullet wall
        }
        else if (randomValue >= 60 && randomValue < 100)
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
                nextAttack = weapons[1];    // Circular attack
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
        if (numBursts == 3)
        {
            nextAttack = weapons[4];    // Deploys a drone
            numBursts = 0;
        }
    }
#endregion

    // Disables the weapon
    void DisableWeapon()
    {
        previousAttack.SetActive(false);
        if (numAttacks > numberOfAttacks)
        {
            ChangeAttack();
            cooldown = timeBetweenBursts + randomTime;
            EnableMovement();
            numAttacks = 0;
            numBursts++;
        }
        else if (numAttacks == numberOfAttacks)
        {
            DeployDrone();
        }
        else
        {
            ChangeAttack();
            cooldown = timeBetweenAttacks;
            readyToAttack = true;
        }
        Invoke("EnableWeapon", cooldown);
    }

    void DisableUltimate()
    {
        previousAttack.SetActive(false);
        SecondUltimate();
    }

    void EnableMovement()
    {
        canRotate = true;
        GetComponent<FollowObject>().enabled = true;
        moveDelay = initialMoveDelay;
    }

    // Changes rotation to face lock on target
    void ChangeRotation()
    {
        // Checks if there is a lock on
        if (lockOn.nearestEnemy != null)
        {
            if (canRotate == true)
            {
                // If currently attacking
                if (numAttacks < numberOfAttacks && GetComponent<FollowObject>().enabled == false)
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
                        transform.rotation = lockOn.defaultRotation * Quaternion.Euler(0, 180f, 0);
                    }
                    else
                    {
                        transform.rotation = lockOn.defaultRotation;
                    }
                }
            }
        }
        else
        {
            transform.rotation = lockOn.defaultRotation;
        }
    }
}
