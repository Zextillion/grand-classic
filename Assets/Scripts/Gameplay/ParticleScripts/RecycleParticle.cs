using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT: Put this script on a particle that is part of an object pool
// Recycles a particle for use in the object pool after it is disabled
public class RecycleParticle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            Recycle();
        }
    }

    void Recycle()
    {
        ObjectPooler.current.CreatePool(gameObject);
    }
}