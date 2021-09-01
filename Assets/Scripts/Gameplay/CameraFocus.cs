using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private float distance = 0f;
    [SerializeField] private float angle = 0f;
    [SerializeField] private GameObject focus = null;

    #endregion

    #region PRIVATE_FIELDS

    private float height = 0f;
    private float smoothSpeed = 0.125f;

    #endregion

    #region UNITY_CALLS

    void Start()
    {
        height = (Mathf.Tan(angle * Mathf.PI / 180)) * distance;
        FollowPlayer();
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    #endregion

    #region PRIVATE_METHODS

    private void FollowPlayer()
    {
        Vector3 target = focus.transform.position + (focus.transform.up * height) - (distance * Vector3.forward);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(focus.transform);
    }

    #endregion
}