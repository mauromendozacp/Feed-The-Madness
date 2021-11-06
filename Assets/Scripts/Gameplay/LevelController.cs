using System;
using UnityEngine;

public enum DIFFICULTY
{
    EASY,
    MEDIUM,
    HARD
}

[Serializable]
public struct LEVEL
{
    public DIFFICULTY difficulty;
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
    [SerializeField] private FloorLoop floorLoop = null;
    [SerializeField] private LEVEL[] difficulties = null;

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

        killer.InitModuleHandlers(lcActions);
        hud.InitModuleHandlers(killer.KActions);
    }

    private void Init()
    {
        killer.Init();

        //AkSoundEngine.PostEvent("mx", gameObject);
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

                floorLoop.Speed += floorLoop.Speed * difficulties[difficultyIndex].increaseSpeed / 100;

                switch (difficulties[difficultyIndex].difficulty)
                {
                    case DIFFICULTY.EASY:

                        obstacleManager.Respawns[1].enabled = true;
                        obstacleManager.Respawns[2].enabled = true;

                        break;
                    case DIFFICULTY.MEDIUM:

                        treeLeftManager.Respawns[2].enabled = true;
                        treeRightManager.Respawns[2].enabled = true;

                        break;
                    case DIFFICULTY.HARD:

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                difficultyIndex++;
            }
        }
    }

    #endregion
}
