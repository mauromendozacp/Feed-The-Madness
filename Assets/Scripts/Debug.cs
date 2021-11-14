using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject debugPanel = null;
    [SerializeField] private float transitionTime = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private bool isOpen = false;
    private bool inTransition = false;

    private bool killerInvincible = false;
    private bool killerUnlimitAxes = false;
    private KActions kActions = null;
    private LCActions lcActions = null;

    #endregion

    #region UNITY_CALLS

    private void Update()
    {
        InputDebug();
    }

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(KActions kActions, LCActions lcActions)
    {
        this.kActions = kActions;
        this.lcActions = lcActions;
    }

    public void Invincible()
    {
        killerInvincible = !killerInvincible;
        kActions.OnInvincible?.Invoke(killerInvincible);
    }

    public void UnlimitedAxes()
    {
        killerUnlimitAxes = !killerUnlimitAxes;
        kActions.OnUnlimitAxes?.Invoke(killerUnlimitAxes);
    }

    public void ChangeDifficulty(int index)
    {
        lcActions.OnChangeDifficulty?.Invoke(index);
    }

    #endregion

    #region PRIVATE_METHODS

    private void InputDebug()
    {
        if (Input.GetKeyDown(KeyCode.F2) && !inTransition)
        {
            OpenDebug();
        }
    }

    private void OpenDebug()
    {
        inTransition = true;

        IEnumerator TransitionDebug(bool open)
        {
            float timer = 0f;
            while (timer < transitionTime)
            {
                timer += Time.deltaTime;

                Vector3 scale = debugPanel.transform.localScale;
                scale.y = Mathf.Lerp(
                    open ? 0f : 1f,
                    open ? 1f : 0f,
                    timer / transitionTime);
                debugPanel.transform.localScale = scale;

                yield return new WaitForEndOfFrame();
            }

            isOpen = open;
            inTransition = false;
            yield return null;
        }

        StartCoroutine(TransitionDebug(!isOpen));
    }

    #endregion
}
