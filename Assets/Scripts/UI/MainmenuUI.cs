using UnityEngine;
using TMPro;

public class MainmenuUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject creditsPanel = null;
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private Loading loading = null;
    [SerializeField] private TMP_Text versionText = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        versionText.text = "v" + Application.version;
        AkSoundEngine.SetState("mx_switch", "mx_menu");
        Cursor.lockState = CursorLockMode.None;
        AkSoundEngine.SetRTPCValue("frenesi", 100f);
    }

    #endregion

    #region PUBLIC_METHODS

    public void PlayGame()
    {
        menuPanel.SetActive(false);
        loading.gameObject.SetActive(true);
        loading.LoadScene("Gameplay");
    }

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
