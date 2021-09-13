using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] protected float jumpForce = 0f;
    [SerializeField] protected float jumpTimer = 0f;
    [SerializeField] protected float jumpDistance = 0f;
    [SerializeField] protected LayerMask obstacleMask = 0;

    #endregion

    #region PROTECTED_FIELDS

    protected bool jumping = false;
    protected bool dead = false;
    protected Rigidbody rigid = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {

    }

    public virtual void Update()
    {
        
    }

    #endregion

    #region PRIVATE_METHODS

    protected void Jump()
    {
        if (!jumping)
        {
            jumping = true;
            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(RestartJump), jumpTimer);
        }
    }

    private void RestartJump()
    {
        jumping = false;
    }

    private IEnumerator JumpMove(Vector3 jumpPoint)
    {
        while (transform.position.y < jumpPoint.y)
        {
            transform.Translate(transform.up * jumpForce * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    #endregion
}
