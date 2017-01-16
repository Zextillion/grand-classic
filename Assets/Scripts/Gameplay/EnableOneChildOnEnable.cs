using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOneChildOnEnable : MonoBehaviour
{
    public GameObject[] objects;
	
	void OnEnable()
    {
        int randomNumber = 0;
        randomNumber = (int)Mathf.Floor(Random.value * objects.Length);

        for (int i = 0; i < objects.Length; i++)
        {
            if (i == randomNumber)
            {
                objects[i].SetActive(true);
            }
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
