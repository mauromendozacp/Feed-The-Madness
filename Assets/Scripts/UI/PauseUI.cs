using UnityEngine;

public class PauseUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject optionsPanel;

    #endregion

    #region PRIVATE_FIELDS

    private bool paused = false;

    #endregion

    #region PUBLIC_METHODS

    public void Pause()
    {
        if (!paused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        AkSoundEngine.SetState("pause", "on");
        paused = true;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        AkSoundEngine.SetState("pause", "off");
        paused = false;
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
        AkSoundEngine.PostEvent("back_to_menu", gameObject);
        AkSoundEngine.SetState("pause", "off");
        GameManager.Get().ChangeScene(SceneGame.MainMenu);
    }

    #endregion
}
