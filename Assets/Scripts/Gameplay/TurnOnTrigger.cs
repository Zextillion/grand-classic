using UnityEngine;
using System.Collections;

// Disables all scripts, then reenables all scripts on trigger
public class TurnOnTrigger : MonoBehaviour
{
    [SerializeField]
    float timeBetweenEnable = 0.05f;

	void OnTriggerEnter2D()
	{
        GetComponent<BoxCollider2D>().enabled = false;
        InvokeRepeating("TurnOn", timeBetweenEnable, timeBetweenEnable);
	}

    void TurnOn()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
