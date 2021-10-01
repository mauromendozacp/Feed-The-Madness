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
    [SerializeField] private ParticleSystem blood = null;

    #endregion

    #region PRIVATE_FIELDS

    private float checkDistance = 5f;
    private Vector3 startPos = Vector3.zero;
    private CapsuleCollider capsuleCollider = null;
    private MovableObject movable = null;

    #endregion

    #region PROPERTIES

    public float CrazinessPoints => crazinessPoints;

    public int Points => points;

    #endregion

    #region UNITY_CALLS

    private void Awake()
    {
        movable = GetComponent<MovableObject>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();

        startPos = transform.position;
        startPos.y -= capsuleCollider.height / 4;
    }

    public override void Update()
    {
        base.Update();

        if (!dead)
        {
            CheckObstacle();
        }
    }

    private void OnEnable()
    {
        Init();
    }

    #endregion

    #region PUBLIC_METHODS

    public void Death()
    {
        if (!dead)
        {
            dead = true;

            anim.SetBool("Death", true);
            blood.Play();
            rigid.useGravity = false;
            capsuleCollider.isTrigger = true;

            Invoke(nameof(DestroySurvivor), deathTimer);
        }
    }

    #endregion

    #region PRIVATE_METHODS

    private void Init()
    {
        dead = false;
        anim.SetBool("Death", false);
        rigid.useGravity = true;
        capsuleCollider.isTrigger = false;
    }

    private void CheckObstacle()
    {
        if (Physics.Raycast(startPos, Vector3.forward, out RaycastHit hit, checkDistance, obstacleMask))
        {
            Vector3 hitPos = hit.transform.position;
            if (transform.position.x < hitPos.x)
            {
                transform.Translate(Vector3.left * (horizontalSpeed * Time.deltaTime));
            }
            else
            {
                transform.Translate(Vector3.right * (horizontalSpeed * Time.deltaTime));
            }
        }
    }

    private void DestroySurvivor()
    {
        movable.Destroy();
    }

    #endregion
}
