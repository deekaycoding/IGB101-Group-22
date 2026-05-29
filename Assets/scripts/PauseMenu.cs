using UnityEngine;
using UnityEngine.SceneManagement;

// Esc toggles a pause panel and freezes time. Resume unfreezes; Quit returns to the menu.
// If the settings panel is open, Esc backs out of it to the pause menu first.
public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject settingsPanel;
    public string menuScene = "MainMenu";

    private bool paused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // when settings is open, Esc just closes it and leaves us on the pause menu
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
                return;
            }
            paused = !paused;
            Apply();
        }
    }

    void Apply()
    {
        if (panel != null) panel.SetActive(paused);
        // never leave the settings overlay up once we're back in the game
        if (!paused && settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = paused ? 0f : 1f;
        // free the mouse to click the menu while paused, recapture it on resume
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    public void Resume()
    {
        paused = false;
        Apply();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
