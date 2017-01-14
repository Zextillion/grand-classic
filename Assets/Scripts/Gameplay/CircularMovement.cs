using UnityEngine;
using System.Collections;

public class CircularMovement : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    float speed = 1.0f;

    void FixedUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}