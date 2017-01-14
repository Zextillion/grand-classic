using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour
{
    [SerializeField]
    GameObject objectToDestroy;             // The object to recycle
    [SerializeField]
    GameObject destroyEffect;
    [SerializeField]
    GameObject drop;

    public void DestroyGameObject ()
    {
        Instantiate(destroyEffect, transform.position, transform.rotation);
        if (drop != null)
        {   // If enemy drops something on death
            Instantiate(drop, transform.position, drop.transform.rotation);
        }
        Destroy(objectToDestroy);
	}
}
