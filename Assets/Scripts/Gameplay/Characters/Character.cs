using UnityEngine;

public class Character : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] protected float horizontalSpeed = 0f;
    [SerializeField] protected float jumpForce = 0f;
    [SerializeField] protected float gravityMultiplier = 0f;
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
        groundCheckDistance = height / 2 + 0.5f;
    }

    #endregion

    #region PROTECTED_METHODS

    protected virtual void Jump()
    {
        if (!isGrounded)
            return;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    protected void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, groundCheckDistance);

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
