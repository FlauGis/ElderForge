using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject aboutUsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        settingsPanel.SetActive(false);
        aboutUsPanel.SetActive(false);
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(true);
        aboutUsPanel.SetActive(false);
    }

    public void ToggleAboutUs()
    {
        aboutUsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
