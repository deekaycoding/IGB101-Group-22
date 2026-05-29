using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Central nexus for the level's game logic: tracks samples collected, decides when
// the level is complete, drives the on-screen counter, and plays nearby audio samples.
public class GameManager : MonoBehaviour
{
    public GameObject player;

    public int currentPickups = 0;
    public int maxPickups = 3;
    public int gateThreshold = 5;       // collect this many to power the gate lever
    public bool levelComplete = false;
    public bool LeverPulled = false;    // set when the player pulls the gate lever

    public bool GateOpen { get { return currentPickups >= gateThreshold; } }

    public Text pickupCounter;          // the "Samples: x/y" UI text
    public Text objectiveText;          // objective / status line

    public AudioSource[] audioSources;  // sample audio sources in the scene
    public float audioProximity = 5f;

    void Start()
    {
        // keep the mouse captured during play so it can't leave the window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LevelCompleteCheck();
        UpdateGUI();
        PlayAudioSamples();
    }

    void LevelCompleteCheck()
    {
        if (currentPickups >= maxPickups)
            levelComplete = true;
    }

    void UpdateGUI()
    {
        if (pickupCounter != null)
            pickupCounter.text = "Samples: " + currentPickups + "/" + maxPickups;
        if (objectiveText != null)
        {
            if (levelComplete)
                objectiveText.text = "All artifacts collected - reach the exit!";
            else if (LeverPulled)
                objectiveText.text = "The gate is open - collect the rest beyond it";
            else if (GateOpen)
                objectiveText.text = "Pull the lever to open the gate";
            else
                objectiveText.text = "Collect " + gateThreshold + " artifacts to power the lever";
        }
    }

    void PlayAudioSamples()
    {
        if (player == null)
            return;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] == null)
                continue;

            if (Vector3.Distance(player.transform.position, audioSources[i].transform.position) <= audioProximity)
            {
                if (!audioSources[i].isPlaying)
                    audioSources[i].Play();
            }
        }
    }
}
