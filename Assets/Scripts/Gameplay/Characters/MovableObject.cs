using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private float speed = 0f;
    [SerializeField] private LayerMask limitMask = default;

    #endregion

    #region PROPERTIES

    public int Index { get; set; } = 0;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float LimitZ { get; set; } = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private MOMActions momActions = null;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        
    }

    void Update()
    {
        MoveBack();
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(MOMActions momActions)
    {
        this.momActions = momActions;
    }

    public void Destroy()
    {
        momActions?.OnRemove?.Invoke(this);
        momActions?.OnReturnPoolManager?.Invoke(gameObject);

        if (momActions == null)
            Destroy(gameObject);
    }

    #endregion

    #region PRIVATE_METHODS

    private void MoveBack()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));

        if (transform.position.z < LimitZ)
            Destroy();
    }

    #endregion
}
