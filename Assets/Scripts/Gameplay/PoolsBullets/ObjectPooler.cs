using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic pooler class
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler current;

    // The object prefabs which the pool can handle.
    [SerializeField]
    GameObject[] objectPrefabs;

    // The pooled objects currently available.
    [SerializeField]
    List<GameObject>[] pooledObjects;

    // The amount of objects of each type to buffer.
    [SerializeField]
    int[] amountToBuffer;
    [SerializeField]
    int defaultBufferAmount = 3;

    // The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
    protected GameObject containerObject;

    // Initializes variables
    void Awake()
    {
        current = this;
        containerObject = gameObject;
    }

	// Use this for initialization
	void Start()
    {
        //Loop through the object prefabs and make a new list for each one.
        //We do this because the pool can only support prefabs set to it in the editor,
        //so we can assume the lists of pooled objects are in the same order as object prefabs in the array
        pooledObjects = new List<GameObject>[objectPrefabs.Length];

        int i = 0;
        foreach (GameObject objectPrefab in objectPrefabs)
        {
            pooledObjects[i] = new List<GameObject>();

            int bufferAmount;

            if (i < amountToBuffer.Length)
            {
                bufferAmount = amountToBuffer[i];
            }
            else
            {
                bufferAmount = defaultBufferAmount;
            }

            for (int j = 0; j < bufferAmount; j++)
            {
                GameObject newObj = Instantiate(objectPrefab) as GameObject;
                newObj.name = objectPrefab.name;
                CreatePool(newObj);
            }

            i++;
        }
	}

    // Creates a pool
    public void CreatePool(GameObject obj)
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == obj.name)
            {
                obj.SetActive(false);
                obj.transform.parent = containerObject.transform;
                pooledObjects[i].Add(obj);
                return;
            }
        }
    }

    // Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool then null will be returned.
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='objectType'>
    /// Object type.
    /// </param>
    /// <param name='willGrow'>
    /// If false, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetObjectForType(string objectType, bool willGrow)
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject prefab = objectPrefabs[i];
            if (prefab.name == objectType)
            {

                if (pooledObjects[i].Count > 0)
                {
                    GameObject pooledObject = pooledObjects[i][0];
                    pooledObjects[i].RemoveAt(0);
                    pooledObject.transform.parent = gameObject.transform;
                    pooledObject.SetActive(true);

                    return pooledObject;
                }
                else if (willGrow)
                {   // If the pool should grow, then grow if too many instances are in play at the same time
                    GameObject obj;
                    obj = Instantiate(objectPrefabs[i]);
                    obj.transform.parent = gameObject.transform;
                    obj.name = objectType;
                    return obj;
                }

                break;

            }
        }

        //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
        return null;
    }
}
