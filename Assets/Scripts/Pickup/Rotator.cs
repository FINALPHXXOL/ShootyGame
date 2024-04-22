using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 50f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate around the Y-axis at 'speed' degrees per second.
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
