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
    [SerializeField] private SurvivorManager survivorManager = null;
    [SerializeField] private ObstacleManager obstacleManager = null;
    [SerializeField] private FloorLoop floorLoop = null;

    [SerializeField] private Transform spawn = null;
    [SerializeField] private float maxX = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private LCActions lcActions = null;
    private bool endLevel = false;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        InitModuleHandlers();
    }

    private void Update()
    {
        if (!endLevel)
        {
            SpawnSurvivors();
            SpawnObstacles();

            MoveObstacles();
            MoveFloors();
            MoveSurvivors();
        }
    }

    #endregion

    #region PRIVATE_FIELDS

    private void InitModuleHandlers()
    {
        lcActions = new LCActions();

        lcActions.OnKillerDead += EndLevel;

        killer.InitModuleHandlers(lcActions);
    }

    private void SpawnSurvivors()
    {
        if (!survivorManager.SpawnActivated)
        {
            survivorManager.SpawnSurvivor(GetRandomPosition());
        }
    }

    private void SpawnObstacles()
    {
        if (!obstacleManager.SpawnActivated)
        {
            obstacleManager.SpawnObstacles(GetRandomPosition());
        }
    }

    private void Move(GameObject obj, float speed)
    {
        obj.transform.Translate((-transform.forward) * (speed * Time.deltaTime));
    }

    private void MoveObstacles()
    {
        foreach (Obstacle obstacle in obstacleManager.ObstacleList)
        {
            Move(obstacle.gameObject, obstacle.Speed);
        }
    }

    private void MoveSurvivors()
    {
        foreach (Survivor survivor in survivorManager.Survivors)
        {
            Move(survivor.gameObject, survivor.Speed);
        }
    }

    private void MoveFloors()
    {
        foreach (GameObject obj in floorLoop.Floors)
        {
            Move(obj, floorLoop.Speed);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = spawn.position;
        pos.x = UnityEngine.Random.Range(pos.x - maxX, pos.x + maxX);

        return pos;
    }

    private void EndLevel()
    {
        endLevel = true;
    }
    #endregion
}
