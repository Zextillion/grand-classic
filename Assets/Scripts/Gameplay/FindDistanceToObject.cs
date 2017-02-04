using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Distance formula
public class FindDistanceToObject : MonoBehaviour
{
    public static FindDistanceToObject current;
	
    void Awake()
    {
        current = this;
    }
	
    public float FindDistance(GameObject obj1, GameObject obj2)
    {
        float distance;

        distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((obj2.transform.position.x - obj1.transform.position.x), 2) + Mathf.Pow((obj2.transform.position.y - obj1.transform.position.y), 2)));

        return distance;
    }
}
