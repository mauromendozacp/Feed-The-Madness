using System;
using System.Collections.Generic;
using UnityEngine;

public class LCActions
{
    public Action OnKillerDead = null;
}

public class LevelController : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Killer killer = null;
    [SerializeField] private HUD hud = null;

    #endregion

    #region PRIVATE_FIELDS

    private LCActions lcActions = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        GameManager.Get().Init();
        InitModuleHandlers();
        Init();
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

    public void Init()
    {
        killer.Init();
    }

    public void DeInit()
    {
        lcActions.OnKillerDead -= EndLevel;

        hud.DeInitModuleHandlers();
    }

    private void EndLevel()
    {
        GameManager.Get().FinishGame(killer.Score);
    }
    #endregion
}
