using UnityEngine;
using System.Collections;

// For particles that are not in an object pool
// Destroys a particle when it is done
public class DestroyParticleWhenFinished : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
	{
		if(!GetComponent<ParticleSystem>().isPlaying)
		{
			Destroy(gameObject);
		}
	}
}
