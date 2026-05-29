using UnityEngine;

// Raises the portcullis out of the doorway once the player has pulled the gate lever.
// Lowers it back if the condition is somehow undone.
public class GateController : MonoBehaviour
{
    public float raiseHeight = 5f;
    public float speed = 1.8f; // weighty, deliberate raise rather than snapping up

    private GameManager gm;
    private Vector3 closedPos;

    void Start()
    {
        var g = GameObject.FindGameObjectWithTag("GameManager");
        if (g != null) gm = g.GetComponent<GameManager>();
        closedPos = transform.position;
    }

    void Update()
    {
        bool open = gm != null && gm.LeverPulled;
        Vector3 target = open ? closedPos + Vector3.up * raiseHeight : closedPos;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
