using System.Collections;
using UnityEngine;

// The gate lever. Usable only once enough samples are collected (GameManager.GateOpen):
// pulling it plays the character's Pull animation, swings the handle, and lets the gate rise.
// Used too early it shows a hint popup telling the player to collect the artifacts first.
public class LeverInteractable : MonoBehaviour, IInteractable
{
    public Transform handle;                              // the part that swings (lever_1b)
    public Vector3 pulledEuler = new Vector3(45f, 0f, 0f); // toward the player (a pull, not a push)
    public float pullDelay = 1.0f;                        // wait for the hand to reach the lever before it moves
    public float swingTime = 1.0f;                        // slower, weightier throw

    private bool pulled;

    // anchor at the lever's base (player height) so proximity is measured like the samples;
    // the interactor floats the prompt above this point.
    public Transform Anchor { get { return transform; } }

    void OnEnable() { Interactables.All.Add(this); }
    void OnDisable() { Interactables.All.Remove(this); }

    public void Interact(PlayerInteractor interactor)
    {
        if (pulled) return;
        var gm = interactor.Manager;
        if (gm != null && gm.GateOpen)
        {
            pulled = true;
            Interactables.All.Remove(this);             // no more prompt once it's thrown
            // play the pull animation; once the hand reaches the lever (pullDelay), swing
            // the handle and release the gate together so it all reads as one motion
            interactor.DoAction("Pull", pullDelay, () =>
            {
                if (handle != null) StartCoroutine(Swing());
                if (gm != null) gm.LeverPulled = true;
            });
        }
        else
        {
            interactor.ShowLockedHint();
        }
    }

    IEnumerator Swing()
    {
        Quaternion from = handle.localRotation;
        Quaternion to = Quaternion.Euler(pulledEuler) * from;
        float t = 0f;
        while (t < swingTime)
        {
            t += Time.deltaTime;
            handle.localRotation = Quaternion.Slerp(from, to, t / swingTime);
            yield return null;
        }
        handle.localRotation = to;
    }
}
