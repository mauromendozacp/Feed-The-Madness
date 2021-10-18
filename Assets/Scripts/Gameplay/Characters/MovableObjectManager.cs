using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MOMActions
{
    public Action<MovableObject> OnRemove = null;
    public Action<GameObject> OnReturnPoolManager = null;
}

public class MovableObjectManager : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private float minTimerSpawn = 0f;
    [SerializeField] private float maxTimerSpawn = 0f;
    [SerializeField] private float maxX = 0f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private PoolManager poolManager = null;

    #endregion

    #region PRIVATE_FIELDS

    private MOMActions momActions = null;

    #endregion

    #region PROPERTIES

    public List<MovableObject> Movables { get; } = new List<MovableObject>();
    public bool SpawnActivated { get; set; } = false;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        InitModuleHandlers();
    }

    void Update()
    {
        Spawn();
    }

    #endregion

    #region PRIVATE_METHODS

    private void InitModuleHandlers()
    {
        momActions = new MOMActions();

        momActions.OnRemove += Remove;
        momActions.OnReturnPoolManager += poolManager.ReturnObjectToPool;
    }

    private void Spawn()
    {
        if (!SpawnActivated)
        {
            GameObject movableGO = poolManager.GetObjectFromPool();
            MovableObject movable = movableGO.GetComponent<MovableObject>();

            movable.InitModuleHandlers(momActions);
            movable.Speed = speed;
            movableGO.transform.position = GetRandomPosition();

            Movables.Add(movable);

            SpawnActivated = true;
            Invoke(nameof(ResetSpawnActivate), GetRandomTimerSpawn());
        }
    }

    private void Remove(MovableObject movable)
    {
        Movables.Remove(movable);
    }

    private void ResetSpawnActivate()
    {
        SpawnActivated = false;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(pos.x - maxX, pos.x + maxX);

        return pos;
    }

    private float GetRandomTimerSpawn()
    {
        return Random.Range(minTimerSpawn, maxTimerSpawn);
    }

    private PoolManager GetPoolManager()
    {
        return poolManager;
    }

    #endregion
}
