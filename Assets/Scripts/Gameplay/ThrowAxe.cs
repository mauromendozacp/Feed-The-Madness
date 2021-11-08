using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAxe : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject prefab = null;
    [SerializeField] private Transform parent = null;
    [SerializeField] private Transform spawnStart = null;

    #endregion

    #region PRIVATE_FIELDS

    private KActions kActions = null;

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(KActions kActions)
    {
        this.kActions = kActions;
    }

    public void Throw()
    {
        GameObject axeGO = Instantiate(prefab, parent);
        axeGO.transform.position = spawnStart.position;
        axeGO.GetComponent<Axe>().InitModuleHandlers(kActions);
    }

    #endregion
}
