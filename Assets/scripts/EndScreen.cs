using UnityEngine;
using UnityEngine.SceneManagement;

// Shown after the final level is completed. Returns to the menu or quits.
public class EndScreen : MonoBehaviour
{
    public string menuScene = "MainMenu";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
