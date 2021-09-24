using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int length;

    #endregion

    #region PRIVATE_FIELDS

    private Queue<GameObject> pool;

    #endregion

    #region UNITY_CALLS

    void Awake()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < length; i++)
        {
            GameObject objectGO = Instantiate(GetRandomPrefab(), transform, true);

            ReturnObjectToPool(objectGO);
        }
    }

    #endregion

    #region PUBLIC_METHODS

    public GameObject GetObjectFromPool()
    {
        GameObject objectGO = pool.Dequeue();
        objectGO.SetActive(true);

        return objectGO;
    }

    public void ReturnObjectToPool(GameObject objectGO)
    {
        objectGO.SetActive(false);
        pool.Enqueue(objectGO);
    }

    #endregion

    #region PRIVATE_METHODS

    private GameObject GetRandomPrefab()
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    #endregion
}
