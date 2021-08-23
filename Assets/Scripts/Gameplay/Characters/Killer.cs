using System.Collections;
using UnityEngine;

public class Killer : Character
{
    #region EXPOSED_FIELDS
    [SerializeField] private float horizontalSpeed = 0f;
    [SerializeField] private float jumpSpeed = 0f;
    [SerializeField] private float craziness = 0f;
    [SerializeField] private float jumpTimer = 0f;
    [SerializeField] private LayerMask obstacleMask = 0;
    #endregion

    #region PRIVATE_FIELDS
    
    private bool jumping = false;
    private bool dead = false;

    #endregion

    #region PROPERTIES

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

            MoveHorizontal();
            DecreaseCraziness();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!jumping)
        {
            if (Tools.CheckLayerInMask(obstacleMask, other.gameObject.layer))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
                    Jump(obstacle.JumpPoint.position);
                }
            }
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void IncreaseCraziness(float craz)
    {
        craziness += craz;
    }
    #endregion

    #region PRIVATE_METHODS
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
        craziness -= Time.deltaTime;

        if (craziness < 0f)
        {
            dead = true;
        }
    }

    private void Jump(Vector3 jumpPoint)
    {
        jumping = true;
        StartCoroutine(JumpMove(jumpPoint));
        Invoke(nameof(RestartJump), jumpTimer);
    }

    private void RestartJump()
    {
        jumping = false;
    }

    private IEnumerator JumpMove(Vector3 jumpPoint)
    {
        while (transform.position.y < jumpPoint.y)
        {
            transform.Translate(transform.up * jumpSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    #endregion
}
