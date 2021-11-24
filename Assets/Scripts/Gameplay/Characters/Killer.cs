using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class KActions
{
    public Action<int> OnScoreRecieved = null;
    public Action<float, float> OnCrazinessUpdated = null;
    public Action<float, float> OnThrowTimerUpdate = null;
    public Action<float, int> OnKillSurvivor = null;
    public Action<bool> OnInvincible = null;
    public Action<bool> OnUnlimitAxes = null;
    public Action<bool> OnChangeThrowIcon = null;
}

public class Killer : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float craziness = 0f;
    [SerializeField] private float attackDistance = 0f;
    [SerializeField] private float decreaseCrazinessSpeed = 0f;
    [SerializeField] private float throwCooldown = 0f;
    [SerializeField] private ThrowAxe throwAxe = null;
    [SerializeField] private PostProcessManager postProcessManager = null;

    [SerializeField] private Animator killerAnim = null;
    [SerializeField] private Animator cameraAnim = null;

    [SerializeField] private Transform leftLimit = null;
    [SerializeField] private Transform rightLimit = null;

    [SerializeField] private LayerMask survivorMask = default;
    [SerializeField] private LayerMask powerupMask = default;
    [SerializeField] private LayerMask limitMask = default;

    #endregion

    #region PRIVATE_FIELDS

    private int score = 0;
    private bool hitted = false;
    private float invulnerableTimer = 1.2f;
    private float deathTimer = 1.5f;
    private Vector3 centerPos = Vector3.zero;

    private bool crazinessStop = false;
    private float crazinessBase = 0f;
    private float crazinessStopTimer = 5f;
    private float checkHorDistance = 2f;

    private bool attackAvailable = false;
    private bool throwAvailable = false;
    private bool throwInAnimation = false;
    private bool isInvincible = false;
    private bool isUnlimitAxes = false;

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
            if (!isInvincible && !Dead)
            {
                if (value > 0)
                {
                    craziness = value < crazinessBase ? value : crazinessBase;
                }
                else
                {
                    craziness = 0;
                    Dead = true;
                }
            }

            kActions.OnCrazinessUpdated?.Invoke(crazinessBase, craziness);
            postProcessManager.ChangeChromatic(craziness, crazinessBase);
        }
    }

    public float DecreaseCrazinessSpeed
    {
        get => decreaseCrazinessSpeed;
        set => decreaseCrazinessSpeed = value;
    }

    public KActions KActions => kActions;

    #endregion

    #region UNITY_CALLS

    protected override void Awake()
    {
        base.Awake();

        postProcessManager.Init();
        origGroundCheckDistance = capsule.height * 15 / 16;
    }

    private void Start()
    {
        Steps();

        centerPos = transform.position;
        AkSoundEngine.SetRTPCValue("frenesi", Craziness);
        AkSoundEngine.PostEvent("cha_heart", gameObject);
    }

    private void Update()
    {
        if (!dead)
        {
            Attack();
            Throw();
            InputJump();
            CheckGroundStatus();
            DecreaseCraziness();
            LocationSound();
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            MoveHorizontal();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckLayerInMask(obstacleMask, collision.gameObject.layer))
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            obstacle.Collision();

            if (hitted)
                return;

            Craziness -= obstacle.Damage;
            Hit();
        }
        else if (Utils.CheckLayerInMask(powerupMask, collision.gameObject.layer))
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

        throwAxe.InitModuleHandlers(kActions);
    }

    public void Init()
    {
        crazinessBase = craziness;
        groundCheckDistance = GetComponent<CapsuleCollider>().height;
        origGroundCheckDistance = groundCheckDistance;

        kActions.OnKillSurvivor = KillSurvivor;
        kActions.OnInvincible = EnableInvincible;
        kActions.OnUnlimitAxes = EnableUnlimitAxes;
    }

    public void KillSurvivor(float craz, int points)
    {
        Craziness += craz;
        Score += points;
    }

    public void ResetAttack()
    {
        attackAvailable = false;
    }

    public void ResetThrowAnimation()
    {
        throwInAnimation = false;
    }

    #endregion

    #region PRIVATE_METHODS

    private void InputJump()
    {
        if (!isGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    protected override void Jump()
    {
        if (!jumping)
            AkSoundEngine.PostEvent("cha_jump", gameObject);

        base.Jump();
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

            rigid.MovePosition(transform.position + dir * (horizontalSpeed * Time.fixedDeltaTime));
        }
    }

    private void Steps()
    {
        IEnumerator StepSoundDelay()
        {
            while (!dead)
            {
                if (!jumping)
                {
                    AkSoundEngine.PostEvent("cha_footsteps", gameObject);
                }

                yield return new WaitForSeconds(0.3f);
            }
        }
        StartCoroutine(StepSoundDelay());
    }

    private void Attack()
    {
        if (attackAvailable || throwInAnimation)
            return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            killerAnim.SetTrigger("Attack");
            AkSoundEngine.PostEvent("cha_axe", gameObject);

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

            StartCoroutine(CastAttack());
        }
    }

    private void Throw()
    {
        if (throwAvailable || attackAvailable)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            killerAnim.SetTrigger("Throw");

            throwAvailable = true;
            throwInAnimation = true;

            IEnumerator ThrowAvailableTimer()
            {
                kActions.OnChangeThrowIcon?.Invoke(false);
                float throwTimer = 0f;

                if (!isUnlimitAxes)
                {
                    while (throwTimer < throwCooldown)
                    {
                        throwTimer += Time.deltaTime;
                        kActions.OnThrowTimerUpdate?.Invoke(throwTimer, throwCooldown);

                        yield return new WaitForEndOfFrame();
                    }
                }

                kActions.OnThrowTimerUpdate?.Invoke(throwCooldown, throwCooldown);
                kActions.OnChangeThrowIcon?.Invoke(true);
                throwAvailable = false;
                AkSoundEngine.PostEvent("ui_cooldown", gameObject);

                yield return null;
            }
            StartCoroutine(ThrowAvailableTimer());
        }
    }

    private void Hit()
    {
        AkSoundEngine.PostEvent("cha_damage", gameObject);

        IEnumerator VignetteEffect()
        {
            float halfTimer = invulnerableTimer / 2;
            float timer = 0f;
            while (timer < halfTimer)
            {
                timer += Time.deltaTime;
                postProcessManager.ChangeVignette(true, timer / halfTimer);
                yield return new WaitForEndOfFrame();
            }

            timer = 0f;
            while (timer < halfTimer)
            {
                timer += Time.deltaTime;
                postProcessManager.ChangeVignette(false, timer / halfTimer);
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }
        StartCoroutine(VignetteEffect());

        hitted = true;
        Invoke(nameof(ResetHitted), invulnerableTimer);
    }

    private void Powerup()
    {
        Craziness = crazinessBase;
        crazinessStop = true;
        AkSoundEngine.PostEvent("cha_boost", gameObject);

        Invoke(nameof(ResetCrazinessStop), crazinessStopTimer);
    }

    private void ResetCrazinessStop()
    {
        crazinessStop = false;
        AkSoundEngine.PostEvent("cha_boost_stop", gameObject);
    }

    private void ResetHitted() => hitted = false;

    private void DecreaseCraziness()
    {
        if (crazinessStop)
            return;

        Craziness -= Time.deltaTime * decreaseCrazinessSpeed;
        AkSoundEngine.SetRTPCValue("frenesi", Craziness);
    }

    private void Death()
    {
        cameraAnim.SetBool("Death", true);
        AkSoundEngine.PostEvent("cha_dead", gameObject);

        IEnumerator VignetteEffect()
        {
            float timer = 0f;
            while (timer < deathTimer)
            {
                timer += Time.deltaTime;
                postProcessManager.ChangeVignette(true, timer / deathTimer);
                yield return new WaitForEndOfFrame();
            }

            lcActions.OnKillerDead?.Invoke();
            yield return null;
        }
        StartCoroutine(VignetteEffect());
    }

    private void EnableInvincible(bool enabled)
    {
        Craziness = crazinessBase;
        isInvincible = enabled;
    }

    private void EnableUnlimitAxes(bool enabled)
    {
        isUnlimitAxes = enabled;
        throwAvailable = false;
    }

    private void LocationSound()
    {
        float locationX = 0;
        if (transform.position.x < centerPos.x)
        {
            locationX = transform.position.x - leftLimit.position.x;
            locationX = -(100 - (locationX * 100 / (centerPos.x - leftLimit.position.x)));
        }
        else
        {
            locationX = rightLimit.position.x - transform.position.x;
            locationX = 100 - (locationX * 100 / (rightLimit.position.x - centerPos.x));
        }

        AkSoundEngine.SetRTPCValue("location", (int)locationX);
    }

    #endregion
}
