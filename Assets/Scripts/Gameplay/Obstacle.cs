using System;
using System.Collections.Generic;
using UnityEngine;

public class OActions
{
    public Action<Obstacle> OnDestroy = null;
}

public class Obstacle : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Transform jumpPoint = null;
    [SerializeField] private float speed = 0f;
    [SerializeField] private LayerMask limitMask = default;

    #endregion

    #region PRIVATE_METHODS

    private OActions oActions = null;

    #endregion

    #region PROPERTIES

    public Transform JumpPoint => jumpPoint;
    public float Speed => speed;
    public OActions OActions => oActions;

    #endregion

    #region UNITY_CALLS

    private void OnCollisionEnter(Collision collision)
    {
        if (Tools.CheckLayerInMask(limitMask, collision.gameObject.layer))
        {
            Destroy();
        }
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers()
    {
        oActions = new OActions();
    }

    public void Destroy()
    {
        oActions.OnDestroy?.Invoke(this);
        Destroy(gameObject);
    }

    #endregion
}
