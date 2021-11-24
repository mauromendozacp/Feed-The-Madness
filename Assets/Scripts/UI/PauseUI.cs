using UnityEngine;

public class PauseUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject optionsPanel;

    #endregion

    #region PUBLIC_METHODS

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        AkSoundEngine.SetState("pause", "on");
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        AkSoundEngine.SetState("pause", "off");
    }

    public void ShowPause()
    {
        gameObject.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        gameObject.SetActive(false);
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
