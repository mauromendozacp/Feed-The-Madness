using UnityEngine;

public class Tools : MonoBehaviour
{
    #region PUBLIC_METHODS

    public static bool CheckLayerInMask(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    #endregion
}
