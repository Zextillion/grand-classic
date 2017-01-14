﻿using UnityEngine;
using System.Collections;

// A specific circular movement script that targets the instance of Gabi currently in the scene
public class CircularMovementGabi : MonoBehaviour
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    float speed = 1.0f;

    void Awake()
    {
        target = GameObject.Find("Gabi");
    }

    void FixedUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}