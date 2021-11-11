using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Transform focus = null;
    [SerializeField] private float cameraSpeed = 10f;

    #endregion

    #region UNITY_CALLS

    void FixedUpdate()
    {
        Vector3 camPos = new Vector3(focus.position.x, focus.position.y + (cameraSpeed * Time.deltaTime), focus.position.z);

        focus.position = Vector3.Lerp(focus.position, camPos, cameraSpeed);
    }

    #endregion
}