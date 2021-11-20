using System;
using UnityEngine;

public class TreeFall : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Animator anim;
    [SerializeField] private float startAnimPosZ = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private bool playAnim = false;
    private bool startSpawn = true;

    #endregion

    #region UNITY_CALLS

    private void OnEnable()
    {
        if (startSpawn)
        {
            startSpawn = false;
            return;
        }

        playAnim = false;
        anim.SetTrigger("off");
        UnityEngine.Debug.Log("idle");
    }

    private void Update()
    {
        Fall();
    }

    #endregion

    #region PRIVATE_METHODS

    private void Fall()
    {
        if (playAnim || !gameObject.activeSelf)
            return;

        if (transform.position.z < startAnimPosZ)
        {
            playAnim = true;
            anim.SetTrigger("fall");
            UnityEngine.Debug.Log("caer");
            AkSoundEngine.PostEvent("amb_falling_tree", gameObject);
        }
    }

    #endregion
}
