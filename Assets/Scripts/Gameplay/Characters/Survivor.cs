using System;
using System.Collections.Generic;
using UnityEngine;

public class SActions
{
    #region ACTIONS
    public Action<Survivor> OnDestroy = null;
    #endregion
}

public class Survivor : Character
{
    #region EXPOSED_FIELDS
    [SerializeField] private float speed = 0f;
    [SerializeField] private float crazinessPoints = 0f;
    [SerializeField] private LayerMask killerMask = default;
    [SerializeField] private LayerMask limitMask = default;
    #endregion

    #region PRIVATE_FIELDS
    private SActions sActions = null;
    #endregion

    #region PROPERTIES
    public float Speed => speed;
    public SActions SActions => sActions;
    #endregion

    #region UNITY_CALLS
    void Start()
    {

    }

    public override void Update()
    {
        base.Update();

        MoveBack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Tools.CheckLayerInMask(killerMask, collision.gameObject.layer))
        {
            Killer killer = collision.gameObject.GetComponent<Killer>();
            killer.IncreaseCraziness(crazinessPoints);
            Destroy();
        }
        else if (Tools.CheckLayerInMask(limitMask, collision.gameObject.layer))
        {
            Destroy();
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void InitModuleHandlers()
    {
        sActions = new SActions();
    }

    public void Destroy()
    {
        sActions.OnDestroy?.Invoke(this);
        Destroy(gameObject);
    }
    #endregion

    #region PRIVATE_METHODS
    private void MoveBack()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime);
    }
    #endregion
}
