using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MOMActions
{
    public Action<MovableObject> OnRemove = null;
}

public class MovableObjectManager : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject prefab = null;
    [SerializeField] private float timerSpawn = 0f;
    [SerializeField] private float maxX = 0f;

    #endregion

    #region PROPERTIES

    public List<MovableObject> Movables { get; } = new List<MovableObject>();
    public bool SpawnActivated { get; set; } = false;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        
    }

    void Update()
    {
        SpawnSurvivor();
    }

    #endregion

    #region PRIVATE_METHODS

    public void SpawnSurvivor()
    {
        if (!SpawnActivated)
        {
            GameObject movableGO = Instantiate(prefab, transform, true);
            MovableObject movable = movableGO.GetComponent<MovableObject>();

            movableGO.transform.position = GetRandomPosition();

            movable.InitModuleHandlers();
            movable.MOMActions.OnRemove += Remove;
            Movables.Add(movable);

            SpawnActivated = true;
            Invoke(nameof(ResetSpawnActivate), timerSpawn);
        }
    }

    private void Remove(MovableObject movable)
    {
        Movables.Remove(movable);
    }

    public void ResetSpawnActivate()
    {
        SpawnActivated = false;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(pos.x - maxX, pos.x + maxX);

        return pos;
    }

    #endregion
}
