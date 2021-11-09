using UnityEngine;

public class Axe : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private float force = 0f;
    [SerializeField] private LayerMask suvivorMask = default;

    #endregion

    #region PRIVATE_FIELDS

    private KActions kActions = null;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Tools.CheckLayerInMask(suvivorMask, other.gameObject.layer))
        {
            Survivor survivor = other.gameObject.GetComponent<Survivor>();

            if (!survivor.Dead)
            {
                survivor.Death();
                kActions.OnKillSurvivor?.Invoke(survivor.CrazinessPoints, survivor.Points);
            }
        }

        Destroy(gameObject);
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(KActions kActions)
    {
        this.kActions = kActions;
    }

    #endregion
}
