using System;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float crazinessPoints = 0f;
    [SerializeField] private int points = 0;

    #endregion

    #region PRIVATE_FIELDS

    private MovableObject movable = null;

    #endregion

    #region PROPERTIES

    public float CrazinessPoints => crazinessPoints;

    public int Points => points;

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
        
    }

    #endregion

    #region PUBLIC_METHODS

    public void Death()
    {
        movable.Destroy();
    }

    #endregion
}
