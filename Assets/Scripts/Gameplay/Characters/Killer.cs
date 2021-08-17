using UnityEngine;

public class Killer : Character
{
    #region EXPOSED_FIELDS
    [SerializeField] private float horizontalSpeed = 0f;
    [SerializeField] private float craziness = 0f;
    [SerializeField] private LayerMask survivorMask = 0;
    #endregion

    #region PRIVATE_FIELDS

    #endregion

    #region PROPERTIES

    #endregion

    #region UNITY_CALLS
    private void Start()
    {

    }

    public override void Update()
    {
        base.Update();

        MoveHorizontal();
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
    #endregion
}
