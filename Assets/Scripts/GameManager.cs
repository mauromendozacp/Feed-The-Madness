using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public int Score { get; set; } = 0;
    public bool GameOver { set; get; } = false;

    public enum SceneGame
    {
        MainMenu,
        GamePlay,
        GameOver
    }

    private void Start()
    {
        AkSoundEngine.PostEvent("mx", gameObject);
    }

    public void ChangeScene(SceneGame scene)
    {
        string sceneName;

        switch (scene)
        {
            case SceneGame.MainMenu:
                sceneName = "MainMenu";
                break;
            case SceneGame.GamePlay:
                sceneName = "Gameplay";
                break;
            case SceneGame.GameOver:
                sceneName = "Gameover";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scene), scene, null);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void Init()
    {
        Score = 0;
        GameOver = false;
    }

    public void FinishGame(int score)
    {
        Score = score;
        GameOver = true;
        ChangeScene(SceneGame.GameOver);
    }
}