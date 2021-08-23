using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject prefab = null;
    [SerializeField] private float timerSpawn = 0f;

    #endregion

    #region PRIVATE_METHODS

    #endregion

    #region PROPERTIES

    public List<Obstacle> ObstacleList { get; } = new List<Obstacle>();
    public bool SpawnActivated { get; set; } = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PUBLIC_METHODS

    public void SpawnObstacles(Vector3 spawnPos)
    {
        GameObject obstacleGO = Instantiate(prefab);
        Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();

        obstacleGO.name = "Obstacle " + (ObstacleList.Count + 1);
        obstacleGO.transform.position = spawnPos;
        obstacleGO.transform.parent = transform;

        obstacle.InitModuleHandlers();
        obstacle.OActions.OnDestroy += DestroyObstacle;
        ObstacleList.Add(obstacle);

        SpawnActivated = true;
        Invoke(nameof(ResetSpawnActivated), timerSpawn);
    }

    #endregion

    #region PRIVATE_METHODS

    private void DestroyObstacle(Obstacle obstacle)
    {
        ObstacleList.Remove(obstacle);
    }

    public void ResetSpawnActivated()
    {
        SpawnActivated = false;
    }

    #endregion
}
