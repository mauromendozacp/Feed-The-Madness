using UnityEngine;

public class PauseUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;

    #endregion

    #region PUBLIC_METHODS

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        AkSoundEngine.SetState("pause", "on");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        AkSoundEngine.SetState("pause", "off");
    }

    public void ShowPause()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        AkSoundEngine.SetState("pause", "off");
        GameManager.Get().ChangeScene(GameManager.SceneGame.MainMenu);
    }

    #endregion
}
