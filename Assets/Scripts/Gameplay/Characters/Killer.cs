using System;
using UnityEngine;

public class KActions
{
    public Action<int> OnScoreRecieved = null;
    public Action<float, float> OnCrazinessUpdated = null;
}

public class Killer : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float horizontalSpeed = 0f;
    [SerializeField] private float craziness = 0f;
    [SerializeField] private Animator anim = null;
    [SerializeField] private LayerMask survivorMask = default;

    #endregion

    #region PRIVATE_FIELDS

    private int score = 0;
    private float crazinessBase = 0f;
    private float animAttackDistance = 5f;

    private KActions kActions = null;
    private LCActions lcActions = null;

    #endregion

    #region PROPERTIES

    public bool Dead
    {
        get => dead;
        set
        {
            dead = value;
            if (dead)
                lcActions.OnKillerDead?.Invoke();
        }
    }

    public int Score
    {
        get => score;
        set
        {
            score = value;
            kActions.OnScoreRecieved?.Invoke(score);
        }
    }

    public float Craziness
    {
        get => craziness;
        set
        {
            if (value > 0)
            {
                if (value < crazinessBase)
                {
                    craziness = value;
                }
                else
                {
                    craziness = crazinessBase;
                }
            }
            else
            {
                craziness = 0;
                Dead = true;
            }
            kActions.OnCrazinessUpdated?.Invoke(crazinessBase, craziness);
        }
    }

    public KActions KActions => kActions;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        if (!dead)
        {
            base.Update();
            AttackAnimation();
            InputJump();

            MoveHorizontal();
            DecreaseCraziness();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Tools.CheckLayerInMask(obstacleMask, collision.gameObject.layer))
        {
            if (!Dead)
                Dead = true;
        }
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(LCActions lcActions)
    {
        kActions = new KActions();
        this.lcActions = lcActions;
    }

    public void Init()
    {
        crazinessBase = craziness;
    }

    public void KillSurvivor(float craz, int points)
    {
        Craziness += craz;
        Score += points;
    }

    #endregion

    #region PRIVATE_METHODS

    private void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void MoveHorizontal()
    {
        if (!jumping)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Vector3 dir = Vector3.zero;

                if (Input.GetKey(KeyCode.A))
                {
                    dir = -transform.right;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    dir = transform.right;
                }

                transform.Translate(dir * (horizontalSpeed * Time.deltaTime));
            }
        }
    }

    private void AttackAnimation()
    {
        if (Physics.Raycast(transform.position, transform.forward, animAttackDistance, survivorMask))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                anim.SetTrigger("Attack");
            }
        }
    }

    private void DecreaseCraziness()
    {
        Craziness -= Time.deltaTime;
    }

    #endregion
}
