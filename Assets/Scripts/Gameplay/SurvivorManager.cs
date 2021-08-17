using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private float timerSpawn = 0f;
    #endregion

    #region PRIVATE FIELDS
    private List<Survivor> survivorsList = new List<Survivor>();
    #endregion

    #region PROPERTIES
    public List<Survivor> Survivors => survivorsList;
    public float TimerSpawn => timerSpawn;
    public bool SpawnActivated { get; set; } = false;
    #endregion

    #region UNITY_CALLS
    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region PUBLIC_METHODS
    public void SpawnSurvivor(Vector3 spawnPos)
    {
        GameObject survivorGO = Instantiate(prefab);
        Survivor survivor = survivorGO.GetComponent<Survivor>();

        survivorGO.name = "Survivor " + (survivorsList.Count + 1);
        survivorGO.transform.position = spawnPos;
        survivorGO.transform.parent = transform;

        survivor.InitModuleHandlers();
        survivor.SActions.OnDestroy += DestroySurvivor;
        survivorsList.Add(survivor);

        SpawnActivated = true;
        Invoke(nameof(ResetSpawnActivate), timerSpawn);
    }

    public void ResetSpawnActivate()
    {
        SpawnActivated = false;
    }
    #endregion

    #region PRIVATE_METHODS
    private void DestroySurvivor(Survivor survivor)
    {
        survivorsList.Remove(survivor);
    }
    #endregion
}
