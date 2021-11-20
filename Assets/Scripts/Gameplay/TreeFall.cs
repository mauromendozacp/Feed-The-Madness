﻿using UnityEngine;

public class TreeFall : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Animator anim;
    [SerializeField] private float fallTimer = 0f;

    #endregion

    #region UNITY_CALLS

    void OnEnable()
    {
        Invoke(nameof(StartFall), fallTimer);
    }

    void OnDisable()
    {
        anim.SetTrigger("off");
    }

    #endregion

    #region PRIVATE_METHODS

    private void StartFall()
    {
        anim.SetTrigger("fall");
        AkSoundEngine.PostEvent("amb_falling_tree", gameObject);
    }

    #endregion
}
