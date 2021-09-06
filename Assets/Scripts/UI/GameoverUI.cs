using System.Collections;
using System.Collections.Generic;
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
    }

    #endregion

    #region PUBLIC_METHODS

    public void RestartGame()
    {
        GameManager.Get().ChangeScene(GameManager.SceneGame.GamePlay);
    }

    public void BackToMenu()
    {
        GameManager.Get().ChangeScene(GameManager.SceneGame.MainMenu);
    }

    #endregion
}
