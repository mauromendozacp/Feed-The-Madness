using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private float speed = 0f;
    [SerializeField] private LayerMask limitMask = default;

    #endregion

    #region PRIVATE_FIELDS

    private MOMActions momActions = null;

    #endregion

    #region PROPERTIES
    public MOMActions MOMActions => momActions;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        
    }

    void Update()
    {
        MoveBack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Tools.CheckLayerInMask(limitMask, collision.gameObject.layer))
        {
            Destroy();
        }
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers()
    {
        momActions = new MOMActions();
    }

    public void Destroy()
    {
        momActions?.OnRemove?.Invoke(this);

        Destroy(gameObject);
    }

    #endregion

    #region PRIVATE_METHODS

    private void MoveBack()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));
    }

    #endregion
}
