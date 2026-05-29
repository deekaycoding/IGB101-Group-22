using UnityEngine;

// Small visual polish for sample pickups: slow spin and a gentle bob so they
// read as collectibles rather than static props.
public class SampleSpin : MonoBehaviour
{
    public float spinSpeed = 60f;
    public float bobHeight = 0.2f;
    public float bobSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
    }
}
