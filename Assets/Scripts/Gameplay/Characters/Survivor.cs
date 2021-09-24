using System;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float crazinessPoints = 0f;
    [SerializeField] private int points = 0;
    [SerializeField] private float deathTimer = 0f;
    [SerializeField] private Animator anim = null;

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
        if (!dead)
        {
            dead = true;
            anim.SetBool("Death", true);
            Invoke(nameof(DestroySurvivor), deathTimer);
        }
    }

    #endregion

    #region PRIVATE_METHODS

    private void DestroySurvivor()
    {
        movable.Destroy();
    }

    #endregion
}
