using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField]
    int maxHealth = 1;
    private int currentHealth = 1;
    private float healthPercent = 100.0f;

    private GameObject circleExplosionSmall;
    private GameObject circleExplosionBig;

	// Use this for initialization
	void Awake ()
    {
        circleExplosionSmall = (GameObject)Resources.Load("circleExplosionSmall");
        circleExplosionBig = (GameObject)Resources.Load("circleExplosionBig");

        currentHealth = maxHealth;
    }

	// Update is called once per frame
	void ApplyDamage (int damage)
    {
        currentHealth -= damage;

        // Health percent
        healthPercent = (int)currentHealth / (int)maxHealth;

        // If enemy is hit, random chance for explosion
        if (Random.value < 0.04 && gameObject.tag == "Enemies")
        {   // Instantiate circle explosion
            GameObject.Instantiate(circleExplosionSmall, transform.position, transform.rotation);
        }

        // Death
        if (currentHealth <= 0.0f)
        {   
            // If enemy is dead, random chance for big explosion
            if (Random.value < 0.02 && gameObject.tag == "Enemies")
            {   // Instantiate circle explosion
                GameObject.Instantiate(circleExplosionBig, transform.position, transform.rotation);
            }
            // Special death script for the player
            if (gameObject.tag != "Player")
            {
                GetComponent<RecycleActor>().Recycle();
            }
            else
            {
                DeathPlayer.current.GetComponent<DeathPlayer>().Death();
                GetComponent<RecycleActor>().Recycle();
            }
        }
    }

    public float CalculateHealthPercent()
    {
        healthPercent = (float)currentHealth / (float)maxHealth * 100;
        return healthPercent;
    }
}
