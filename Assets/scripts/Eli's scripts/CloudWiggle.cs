using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudWiggle : MonoBehaviour
{
    [Header("Wiggle Settings")]
    public float speed = 1.0f;
    public float amplitude = 0.5f;
    public float frequency = 4.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float nextWiggleTime;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        
        if (Time.time > nextWiggleTime)
        {
            
            Vector3 randomOffset = new Vector3(
                Random.Range(-amplitude, amplitude),
                Random.Range(-amplitude, amplitude),
                0 // Set to 0 if you only want 2D (XY) wiggle
            );
            targetPosition = startPosition + randomOffset;
            
            // Set time for next change
            nextWiggleTime = Time.time + (1.0f / frequency);
        }

        // Smoothly move towards the random target
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1.0f / (speed * 10f));
    }
}