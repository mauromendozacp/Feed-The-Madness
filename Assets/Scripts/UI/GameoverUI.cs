using UnityEngine;
using TMPro;

public class GameoverUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private TMP_Text scoreText = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        scoreText.text = "SCORE: " + GameManager.Get().Score + "pts";
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

    #region PUBLIC_METHODS

    public void RestartGame()
    {
        GameManager.Get().ChangeScene(SceneGame.GamePlay);
    }

    public void BackToMenu()
    {
        GameManager.Get().ChangeScene(SceneGame.MainMenu);
    }

    #endregion
}
