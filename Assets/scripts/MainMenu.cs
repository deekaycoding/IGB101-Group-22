using UnityEngine;
using UnityEngine.SceneManagement;

// Title screen controller. Start loads the first level; Quit closes the game.
public class MainMenu : MonoBehaviour
{
    public string firstLevel = "Museum";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
