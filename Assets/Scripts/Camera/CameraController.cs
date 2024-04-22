using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField, Tooltip("The speed the camera rotates and repositions.")]
    public float speed;
    [SerializeField, Tooltip("Distance the camera floats above target.")]
    public float distance;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate where our camera wants to be
            Vector3 newPosition = new Vector3(target.position.x, target.position.y + distance, target.position.z);

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

            // Look at the target
            transform.LookAt(target.position, target.forward);
        }
        if (GameManager.instance.isPaused) return;

        //... the rest of the function goes here.
    }
}