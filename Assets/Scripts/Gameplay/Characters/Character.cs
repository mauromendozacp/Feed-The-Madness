using UnityEngine;

public class Character : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] protected float horizontalSpeed = 0f;
    [SerializeField] protected float jumpForce = 0f;
    [SerializeField] protected float gravityMultiplier = 0f;
    [SerializeField] protected LayerMask terrainMask = default;
    [SerializeField] protected LayerMask obstacleMask = default;

    #endregion

    #region PROTECTED_FIELDS

    protected bool jumping = false;
    protected bool dead = false;
    protected Rigidbody rigid = null;
    protected CapsuleCollider capsule = null;

    protected bool isGrounded = false;
    protected float origGroundCheckDistance = 0f;
    protected float groundCheckDistance = 0.2f;
    private float jumpTimer = 1f;

    #endregion

    #region PROPERTIES

    public virtual bool Dead
    {
        get => dead;
        set => dead = value;
    }

    #endregion

    #region UNITY_CALLS

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        float height = capsule.height;
        origGroundCheckDistance = height * 15 / 16;
    }

    #endregion

    #region PROTECTED_METHODS

    protected virtual void Jump()
    {
        if (!isGrounded || jumping)
            return;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumping = true;
        Invoke(nameof(ResetJump), jumpTimer);
    }

    private void ResetJump() => jumping = false;

    protected void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, groundCheckDistance, terrainMask);

        if (!isGrounded)
            HandleAirborneMovement();
    }

    private void HandleAirborneMovement()
    {
        rigid.AddForce(Physics.gravity * gravityMultiplier);
        groundCheckDistance = rigid.velocity.y < 0 ? origGroundCheckDistance : groundCheckDistance;
    }

    #endregion
}
