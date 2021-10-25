using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class KActions
{
    public Action<int> OnScoreRecieved = null;
    public Action<float, float> OnCrazinessUpdated = null;
}

public class Killer : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float craziness = 0f;
    [SerializeField] private float attackDistance = 0f;
    [SerializeField] private float obstacleCrazDecrease = 25f;
    [SerializeField] private Animator killerAnim = null;
    [SerializeField] private Animator cameraAnim = null;
    [SerializeField] private float decreaseCrazinessVel = 0f;
    [SerializeField] private PostProcessVolume volume = null;

    [SerializeField] private LayerMask survivorMask = default;
    [SerializeField] private LayerMask powerupMask = default;
    [SerializeField] private LayerMask limitMask = default;

    #endregion

    #region PRIVATE_FIELDS

    private int score = 0;
    private float crazinessBase = 0f;
    private float checkHorDistance = 2f;
    private bool attackAvailable = false;
    private float resetAttackTimer = 0.5f;
    private bool hitted = false;
    private bool crazinessStoped = false;
    private float crazinessStopedTimer = 3f;
    private float invulnerableTimer = 1.2f;
    private float deathTimer = 1.5f;
    private CapsuleCollider capsule = null;

    private ChromaticAberration chromatic = null;
    private Vignette vignette = null;
    private float minChromaticValue = 0.2f;
    private float maxChromaticValue = 1f;
    private float minVignetteValue = 0f;
    private float maxVignetteValue = 1f;

    private KActions kActions = null;
    private LCActions lcActions = null;

    #endregion

    #region PROPERTIES

    public override bool Dead
    {
        get => dead;
        set
        {
            dead = value;
            if (dead)
                Death();
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
            chromatic.intensity.value = GetChromaticValue();
        }
    }

    public KActions KActions => kActions;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        origGroundCheckDistance = GetComponent<CapsuleCollider>().height * 11 / 16;

        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out vignette);
    }

    public override void Update()
    {
        if (!dead)
        {
            base.Update();
            MoveHorizontal();
            Attack();
            InputJump();
            CheckGroundStatus();
            DecreaseCraziness();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Tools.CheckLayerInMask(obstacleMask, collision.gameObject.layer))
        {
            Hit();
        }
        else if (Tools.CheckLayerInMask(powerupMask, collision.gameObject.layer))
        {
            collision.gameObject.GetComponent<MovableObject>().Destroy();
            Powerup();
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
        groundCheckDistance = GetComponent<CapsuleCollider>().height;
        origGroundCheckDistance = groundCheckDistance;
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
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }
    }

    private void MoveHorizontal()
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

    private void Attack()
    {
        if (!attackAvailable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                killerAnim.SetTrigger("Attack");

                IEnumerator CastAttack()
                {
                    while (attackAvailable)
                    {
                        if (Physics.Raycast(transform.position, transform.forward, 
                            out RaycastHit hit, attackDistance, survivorMask, QueryTriggerInteraction.UseGlobal))
                        {
                            Survivor survivor = hit.transform.gameObject.GetComponent<Survivor>();

                            if (!survivor.Dead)
                            {
                                survivor.Death();
                                KillSurvivor(survivor.CrazinessPoints, survivor.Points);
                            }
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

    private void Hit()
    {
        if (!hitted)
        {
            Craziness -= obstacleCrazDecrease;

            IEnumerator VignetteEffect()
            {
                float halfTimer = invulnerableTimer / 2;
                float timer = 0f;
                while (timer < halfTimer)
                {
                    timer += Time.deltaTime;
                    vignette.intensity.value = Mathf.Lerp(minVignetteValue, maxVignetteValue, timer / halfTimer);
                    yield return new WaitForEndOfFrame();
                }

                timer = 0f;
                while (timer < halfTimer)
                {
                    timer += Time.deltaTime;
                    vignette.intensity.value = Mathf.Lerp(maxVignetteValue, minVignetteValue, timer / halfTimer);
                    yield return new WaitForEndOfFrame();
                }

                yield return null;
            }
            StartCoroutine(VignetteEffect());

            hitted = true;
            rigid.useGravity = false;
            capsule.isTrigger = true;
            Invoke(nameof(ResetHitted), invulnerableTimer);
        }
    }

    private void Powerup()
    {
        crazinessStoped = true;

        Invoke(nameof(ResetCrazinessStop), crazinessStopedTimer);
    }

    private void ResetHitted()
    {
        hitted = false;
        rigid.useGravity = true;
        capsule.isTrigger = false;
    }

    private void ResetCrazinessStop()
    {
        crazinessStoped = false;
    }

    private void DecreaseCraziness()
    {
        if (!crazinessStoped)
        {
            Craziness -= Time.deltaTime * decreaseCrazinessVel;
        }
    }

    private float GetChromaticValue()
    {
        float crazinessPercent = 100 - craziness * 100 / crazinessBase;
        float chromaticValue = (maxChromaticValue - minChromaticValue) * crazinessPercent / 100 + minChromaticValue;
        return chromaticValue;
    }

    private void Death()
    {
        cameraAnim.SetBool("Death", true);
        IEnumerator VignetteEffect()
        {
            float timer = 0f;
            while (timer < deathTimer)
            {
                timer += Time.deltaTime;
                vignette.intensity.value = Mathf.Lerp(minVignetteValue, maxVignetteValue, timer / deathTimer);
                yield return new WaitForEndOfFrame();
            }

            lcActions.OnKillerDead?.Invoke();
            yield return null;
        }
        StartCoroutine(VignetteEffect());
    }

    #endregion
}
