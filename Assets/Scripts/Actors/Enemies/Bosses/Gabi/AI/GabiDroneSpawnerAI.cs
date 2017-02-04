using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabiDroneSpawnerAI : MonoBehaviour
{
    [SerializeField]
    GameObject[] objects;
	
	void OnEnable()
    {
        float healthPercent;
        healthPercent = WeaponsGabi.current.bossHealth.CalculateHealthPercent();
        // The second ultimate was placed, boss at 50% HP
        if (healthPercent < 50.0f)
        {   // Enable the missile drone
            objects[1].SetActive(true);
        }
        else
        {   // If over 50% HP, enable the regular drone
            objects[0].SetActive(true);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
