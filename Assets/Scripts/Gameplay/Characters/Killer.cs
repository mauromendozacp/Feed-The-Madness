using System;
using System.Collections;
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
    [SerializeField] private float attackDistance = 0f;
    [SerializeField] private Animator anim = null;
    [SerializeField] private LayerMask survivorMask = default;
    [SerializeField] private LayerMask limitMask = default;
    [SerializeField] private float decreaseCrazinessVel = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private int score = 0;
    private float crazinessBase = 0f;
    private float checkHorDistance = 2f;
    private bool attackAvailable = false;
    private float resetAttackTimer = 0.5f;

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
            MoveHorizontal();
            Attack();
            InputJump();

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

                if (Physics.Raycast(transform.position, dir, checkHorDistance, limitMask))
                    return;

                transform.Translate(dir * (horizontalSpeed * Time.deltaTime));
            }
        }
    }

    private void Attack()
    {
        if (!attackAvailable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Attack");

                IEnumerator CastAttack()
                {
                    while (attackAvailable)
                    {
                        if (Physics.Raycast(transform.position, transform.forward, 
                            out RaycastHit hit, attackDistance, survivorMask, QueryTriggerInteraction.UseGlobal))
                        {
                            Survivor survivor = hit.transform.gameObject.GetComponent<Survivor>();
                            survivor.Death();

                            KillSurvivor(survivor.CrazinessPoints, survivor.Points);
                        }

                        yield return new WaitForEndOfFrame();
                    }

                    yield return null;
                }

                attackAvailable = true;
                Invoke(nameof(ResetAttack), resetAttackTimer);

                StartCoroutine(CastAttack());
            }
        }
    }

    private void ResetAttack()
    {
        attackAvailable = false;
    }

    private void DecreaseCraziness()
    {
        Craziness -= Time.deltaTime * decreaseCrazinessVel;
    }

    #endregion
}
