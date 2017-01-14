using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Disables a bullet, putting it back in the bullet pool as an inactive bullet
public class RecycleObject : MonoBehaviour
{
    [SerializeField]
    float timeToRecycle= 10.0f;

    void OnEnable()
    {
        CancelInvoke();
        Invoke("Recycle", timeToRecycle);
    }

    void Recycle()
    {
        ObjectPooler.current.CreatePool(gameObject);
    }
}
