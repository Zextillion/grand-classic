using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;

	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
