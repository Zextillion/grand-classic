using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour
{
    [SerializeField]
    float initialSpeed = 10.0f;

	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.right * initialSpeed * Time.deltaTime;
    }
}
