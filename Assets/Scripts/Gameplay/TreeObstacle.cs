using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private LayerMask survivorMask = default;

    #endregion

    #region PRIVATE_FIELDS

    public bool ColliderOn { get; set; } = false;

    #endregion

    #region UNITY_CALLS

    private void OnCollisionEnter(Collision other)
    {
        if (ColliderOn)
        {
            if (Utils.CheckLayerInMask(survivorMask, other.gameObject.layer))
            {
                Survivor survivor = other.gameObject.GetComponent<Survivor>();
                survivor.Death();
            }
        }
    }

    #endregion
}
