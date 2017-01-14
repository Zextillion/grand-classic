using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spin script
public class Spin : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;
	
	void FixedUpdate ()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
