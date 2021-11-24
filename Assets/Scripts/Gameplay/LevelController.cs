using System;
using UnityEngine;

public enum DIFFICULTY
{
    EASY,
    MEDIUM,
    HARD
}

[Serializable]
public class Level
{
    public DIFFICULTY difficulty = default;
    public float time = 0f;
    public float increaseSpeed = 0f;
    public float decreaseCraziness = 0f;
}

public class LCActions
{
    public Action OnKillerDead = null;
    public Action<int> OnChangeDifficulty = null;
}

public class LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Killer killer = null;
    [SerializeField] private HUD hud = null;
    [SerializeField] private FloorLoop floorLoop = null;
    [SerializeField] private Debug debug = null;
    [SerializeField] private Level[] difficulties = null;

    [Header("Obstacles"), Space]
    [SerializeField] private PoolManager obstacleManager = null;
    [SerializeField] private PoolManager treeLeftManager = null;
    [SerializeField] private PoolManager treeRightManager = null;

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
        lcActions.OnChangeDifficulty += ChangeDifficulty;

        killer.InitModuleHandlers(lcActions);
        hud.InitModuleHandlers(killer.KActions);
        debug.InitModuleHandlers(killer.KActions, lcActions);
    }

    private void Init()
    {
        killer.Init();
        Cursor.lockState = CursorLockMode.Locked;

        AkSoundEngine.SetState("mx_switch", "mx_gameplay");
        AkSoundEngine.PostEvent("amb_wind", gameObject);
        AkSoundEngine.PostEvent("amb_rain", gameObject);
    }

    private void DeInit()
    {
        lcActions.OnKillerDead -= EndLevel;
        lcActions.OnChangeDifficulty -= ChangeDifficulty;

        hud.DeInitModuleHandlers();
    }

    private void EndLevel()
    {
        GameManager.Get().FinishGame(killer.Score);
    }

    private void CheckTimer()
    {
        timer += Time.deltaTime;

        if (difficultyIndex < difficulties.Length)
        {
            if (timer > difficulties[difficultyIndex].time)
            {
                ChangeDifficulty();
                difficultyIndex++;
            }
        }
    }

    private void ChangeDifficulty()
    {
        MovableObjectManager[] movables = FindObjectsOfType<MovableObjectManager>();
        foreach (MovableObjectManager movable in movables)
        {
            movable.Speed = movable.BaseSpeed + movable.BaseSpeed * difficulties[difficultyIndex].increaseSpeed / 100;
            movable.ChangeRangeTimerSpawn(difficultyIndex);
        }
        floorLoop.Speed = floorLoop.BaseSpeed + floorLoop.BaseSpeed * difficulties[difficultyIndex].increaseSpeed / 100;
        killer.DecreaseCrazinessSpeed = difficulties[difficultyIndex].decreaseCraziness;

        switch (difficulties[difficultyIndex].difficulty)
        {
            case DIFFICULTY.EASY:

                obstacleManager.Respawns[1].enabled = true;
                obstacleManager.Respawns[2].enabled = true;

                treeLeftManager.Respawns[2].enabled = false;
                treeRightManager.Respawns[2].enabled = false;

                break;
            case DIFFICULTY.MEDIUM:

                treeLeftManager.Respawns[2].enabled = true;
                treeRightManager.Respawns[2].enabled = true;

                break;
            case DIFFICULTY.HARD:

                treeLeftManager.Respawns[2].enabled = true;
                treeRightManager.Respawns[2].enabled = true;

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeDifficulty(int index)
    {
        if (index < difficulties.Length)
        {
            difficultyIndex = index;
            ChangeDifficulty();

            int auxDiffIndex = difficultyIndex;
            timer = auxDiffIndex <= 0 ? 0f : difficulties[auxDiffIndex].time;
        }
    }

    #endregion
}
