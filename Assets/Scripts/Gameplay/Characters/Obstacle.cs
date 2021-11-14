using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private float damage = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private bool collision = false;
    private Rigidbody rigid = null;
    private BoxCollider boxCollider = null;

    #endregion

    #region PROPERTIES

    public float Damage => damage;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        if (!collision)
            return;

        collision = false;
        boxCollider.isTrigger = false;

        if (rigid != null)
            rigid.useGravity = true;
    }

    #endregion

    #region PUBLIC_METHODS

    public void Collision()
    {
        if (collision)
            return;

        collision = true;
        boxCollider.isTrigger = true;

        if (rigid != null)
            rigid.useGravity = false;
    }

    #endregion
}
