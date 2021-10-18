using System;
using UnityEngine;

[Serializable]
public struct DIFFICULTY
{
    public float time;
    public float increaseSpeed;
}

public class LCActions
{
    public Action OnKillerDead = null;
}

public class LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Killer killer = null;
    [SerializeField] private HUD hud = null;
    [SerializeField] private DIFFICULTY[] difficulties = null;

    #endregion

    #region PRIVATE_FIELDS

    private LCActions lcActions = null;
    private float timer = 0f;
    private int difficultyIndex = 0;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        GameManager.Get().Init();
        InitModuleHandlers();
        Init();
    }

    private void Update()
    {
        CheckTimer();
    }

    private void OnDestroy()
    {
        DeInit();
    }

    #endregion

    #region PRIVATE_FIELDS

    private void InitModuleHandlers()
    {
        lcActions = new LCActions();

        lcActions.OnKillerDead += EndLevel;

        killer.InitModuleHandlers(lcActions);
        hud.InitModuleHandlers(killer.KActions);
    }

    private void Init()
    {
        killer.Init();
    }

    private void DeInit()
    {
        lcActions.OnKillerDead -= EndLevel;

        hud.DeInitModuleHandlers();
    }

    private void EndLevel()
    {
        GameManager.Get().FinishGame(killer.Score);
    }

    private void CheckTimer()
    {
        timer += Time.deltaTime;

        if (difficultyIndex < difficulties.Length - 1)
        {
            if (timer > difficulties[difficultyIndex].time)
            {
                MovableObjectManager[] movables = FindObjectsOfType<MovableObjectManager>();
                foreach (MovableObjectManager movable in movables)
                {
                    movable.Speed += movable.Speed * difficulties[difficultyIndex].increaseSpeed / 100;
                }

                difficultyIndex++;
            }
        }
    }

    #endregion
}
