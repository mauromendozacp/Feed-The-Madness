using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] protected float horizontalSpeed = 0f;
    [SerializeField] protected float jumpForce = 0f;
    [SerializeField] protected float jumpTimer = 0f;
    [SerializeField] protected float jumpDistance = 0f;
    [SerializeField] protected LayerMask obstacleMask = default;

    #endregion

    #region PROTECTED_FIELDS

    protected bool jumping = false;
    protected bool dead = false;
    protected Rigidbody rigid = null;

    protected bool isGrounded = false;
    protected float origGroundCheckDistance = 0f;
    protected float groundCheckDistance = 0.2f;
    protected float gravityMultiplier = 5f;

    #endregion

    #region PROPERTIES

    public virtual bool Dead
    {
        get => dead;
        set => dead = value;
    }

    #endregion

    #region UNITY_CALLS

    private void Start()
    {

    }

    public virtual void Update()
    {
        
    }

    #endregion

    #region PROTECTED_METHODS

    protected void Jump()
    {
        if (!jumping)
        {
            jumping = true;
            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(RestartJump), jumpTimer);
        }
    }

    protected void RestartJump()
    {
        jumping = false;
    }

    protected void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out _, groundCheckDistance);

        if (!isGrounded)
        {
            HandleAirborneMovement();
        }
    }

    private void HandleAirborneMovement()
    {
        Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        rigid.AddForce(extraGravityForce);

        groundCheckDistance = rigid.velocity.y < 0 ? origGroundCheckDistance : groundCheckDistance;
    }

    #endregion
}
