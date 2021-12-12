using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RESPAWN
{
    public GameObject prefab;
    public float percent;
    public int length;
    public bool once;
    public bool enabled;
}

public class PoolManager : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private RESPAWN[] respawns;

    #endregion

    #region PRIVATE_FIELDS

    private float onceTimer = 15f;
    private float limitZ = -6f;
    private Queue<GameObject>[] pool;

    #endregion

    #region PROPERTIES

    public RESPAWN[] Respawns => respawns;

    #endregion

    #region UNITY_CALLS

    void Awake()
    {
        pool = new Queue<GameObject>[respawns.Length];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = new Queue<GameObject>();

            for (int j = 0; j < respawns[i].length; j++)
            {
                GameObject objectGO = Instantiate(respawns[i].prefab, transform, true);
                MovableObject obj = objectGO.GetComponent<MovableObject>();
                obj.Index = i;
                obj.LimitZ = limitZ;

                ReturnObjectToPool(objectGO);
            }
        }
    }

    #endregion

    #region PUBLIC_METHODS

    public GameObject GetObjectFromPool()
    {
        int index = GetRandomIndex();
        GameObject objectGO = pool[index].Dequeue();
        objectGO.SetActive(true);
        objectGO.GetComponent<MovableObject>().Index = index;

        return objectGO;
    }

    public void ReturnObjectToPool(GameObject objectGO)
    {
        int index = objectGO.GetComponent<MovableObject>().Index;
        objectGO.SetActive(false);
        pool[index].Enqueue(objectGO);
    }

    #endregion

    #region PRIVATE_METHODS

    private int GetRandomIndex()
    {
        int percent = UnityEngine.Random.Range(1, 101);
        int index = 0;

        for (int i = 0; i < respawns.Length; i++)
        {
            if (percent < respawns[i].percent
                && respawns[i].enabled)
            {
                if (respawns[i].once)
                {
                     respawns[i].enabled = false;
                    IEnumerator RestartOnce()
                    {
                        yield return new WaitForSeconds(onceTimer);
                        respawns[i].enabled = true;
                    }
                    StartCoroutine(RestartOnce());
                }
                index = i;

                break;
            }
        }

        return index;
    }

    #endregion
}
