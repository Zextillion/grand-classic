using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Disables an enemy
// IMPORTANT: Make sure that if using this method to disable an enemy, the object's name perfectly matches a prefab in the object pool
public class RecycleActor : MonoBehaviour
{
    [SerializeField]
    GameObject objectToDestroy;             // The object to recycle
    [SerializeField]
    GameObject destroyEffect;
    private string destroyEffectName;       // The effect
    private bool willGrow = true;           // If set to true, grow the object pool if there are too many instances

    [SerializeField]
    GameObject drop;

    // Initialization
    void Awake()
    {
        destroyEffectName = destroyEffect.transform.name;
    }

    public void Recycle()
    {
        // Gets a gib particle from the pool
        GameObject obj = ObjectPooler.current.GetObjectForType(destroyEffectName, willGrow);
        // If there is no destroy effect, don't display a destroy effect
        if (obj == null)
        {
            return;
        }
        else
        {   // Else display a destroy effect
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }

        // If enemy drops something on death
        if (drop != null)
        {   
            Instantiate(drop, transform.position, drop.transform.rotation);
        }

        if (LockOn.current.nearestEnemy == gameObject)
        {
            LockOn.current.shouldLockOn = true;
        }

        ObjectPooler.current.CreatePool(objectToDestroy);
    }
}
