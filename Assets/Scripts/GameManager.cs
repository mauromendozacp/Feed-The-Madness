using System;
using UnityEngine.SceneManagement;

#region ENUMS

public enum SceneGame
{
    MainMenu,
    GamePlay,
    GameOver
}

#endregion

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    #region PROPERTIES

    public int Score { get; set; } = 0;
    public bool GameOver { set; get; } = false;
    public Sounds Sound { get; set; } = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        AkSoundEngine.PostEvent("mx", gameObject);
    }

    #endregion

    #region PUBLIC_METHODS

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

    public void InitSound(float sfx, float music)
    {
        Sound = new Sounds {Sfx = sfx, Music = music};
    }

    #endregion
}