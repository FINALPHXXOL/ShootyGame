using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    public float oscillationAmplitude = 0.5f; // Amplitude of the up/down movement
    public float oscillationFrequency = 2f; // Frequency of the up/down movement

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        // Record the starting position of the GameObject
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle vertical oscillation
        float newY = startPosition.y + oscillationAmplitude * Mathf.Sin(Time.time * oscillationFrequency);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
