using UnityEngine;
using System.Collections;

public class SpawnWave : MonoBehaviour
{
    [SerializeField]
    GameObject objectToSpawn;       // The object prefab to spawn
    [SerializeField]
    float spawnDelay = 1.0f;        // The delay in between spawn times
	public int numberOfObjects;     // How many enemies to spawn

	private int objectCount;         // How many enemies to spawn

    private bool willGrow = true;   // Set true if you want to expand the pool if there are too many instances of the object
    private string objectType;      // The name of the enemy prefab

    // Initializes prefab
    void Awake()
    {
        objectType = objectToSpawn.transform.name;
    }

    // Use this for initialization
    void Start ()
	{
		objectCount = numberOfObjects;
		Spawn();
	}

	void Spawn()
	{
		objectCount--;
        // Gets an object from the pool
        GameObject obj = ObjectPooler.current.GetObjectForType(objectType, willGrow);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.parent = transform;
		if(objectCount > 0)
		{
            Invoke("Spawn", spawnDelay);
		}
	}

    // Debug, allows editor to see where a spawner is
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, Vector3.one);
	}
}
