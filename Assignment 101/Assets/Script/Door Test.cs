using UnityEngine;

public class DoorTest : MonoBehaviour
{
    Animation animation;

    void Start()
    {
        animation = GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            animation.Play();
        }
    }
}
