using UnityEngine;
using UnityEngine.UI;

// Settings panel shared by the main menu and the in-level pause menu. Two sliders:
// mouse look sensitivity and master volume. Both are saved in PlayerPrefs so the
// choice sticks between scenes and play sessions.
public class SettingsMenu : MonoBehaviour
{
    public GameObject panel;
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    public const string SensKey = "MouseSensitivity";
    public const string VolKey = "MasterVolume";
    const float DefaultSensitivity = 5f;

    void Awake()
    {
        // apply the saved volume straight away, before the panel is ever opened
        AudioListener.volume = PlayerPrefs.GetFloat(VolKey, 1f);

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat(SensKey, DefaultSensitivity);
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        if (panel != null) panel.SetActive(false);
    }

    public void Open() { if (panel != null) panel.SetActive(true); }
    public void Close() { if (panel != null) panel.SetActive(false); }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SensKey, value);
        PlayerPrefs.Save();
        // if a camera is live (pause menu), apply it straight away
        var cam = FindObjectOfType<ThirdPersonCamera>();
        if (cam != null) cam.cameraSpeed = value;
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolKey, value);
        PlayerPrefs.Save();
    }
}
