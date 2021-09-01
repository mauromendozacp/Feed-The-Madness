using System.Collections;
using UnityEngine;

public class Killer : Character
{
    #region EXPOSED_FIELDS

    [SerializeField] private float horizontalSpeed = 0f;
    [SerializeField] private float craziness = 0f;

    #endregion

    #region PRIVATE_FIELDS

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
            {
                lcActions.OnKillerDead?.Invoke();
            }
        }
    }

    public float Craziness
    {
        get => craziness;
        set
        {
            if (value > 0)
            {
                craziness = value;
            }
            else
            {
                craziness = 0;
                Dead = true;
            }
        }
    }

    #endregion

    #region UNITY_CALLS

    private void Start()
    {

    }

    public override void Update()
    {
        if (!dead)
        {
            base.Update();
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
            {
                Dead = true;
            }
        }
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(LCActions lcActions)
    {
        this.lcActions = lcActions;
    }

    public void IncreaseCraziness(float craz)
    {
        Craziness += craz;
    }

    #endregion

    #region PRIVATE_METHODS

    private void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
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

                transform.Translate(dir * horizontalSpeed * Time.deltaTime);
            }
        }
    }

    private void DecreaseCraziness()
    {
        Craziness -= Time.deltaTime;
    }

    #endregion
}
