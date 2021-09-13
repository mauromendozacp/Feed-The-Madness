using System;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float crazinessPoints = 0f;
    [SerializeField] private int points = 0;
    [SerializeField] private LayerMask killerMask = default;

    #endregion

    #region PRIVATE_FIELDS

    private MovableObject movable = null;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        movable = GetComponent<MovableObject>();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Tools.CheckLayerInMask(killerMask, collision.gameObject.layer))
        {
            Killer killer = collision.gameObject.GetComponent<Killer>();
            killer.KillSurvivor(crazinessPoints, points);
            movable.Destroy();
        }
    }

    #endregion
}
