using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The level exit. Once the level is complete and the player enters this trigger,
// it loads the next scene in the sequence.
public class LevelSwitch : MonoBehaviour
{
    private GameManager gameManager;
    public string nextLevel;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.CompareTag("Player") && gameManager.levelComplete)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
