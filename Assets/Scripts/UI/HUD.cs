using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private TMP_Text scoreText = null;
    [SerializeField] private Image crazinessBar = null;
    [SerializeField] private GameObject CrazinessIcon = null;
    [SerializeField] private Transform barStart = null;
    [SerializeField] private Transform barEnd = null;

    #endregion

    #region PRIVATE_FIELDS

    private KActions kActions = null;

    #endregion

    #region PUBLIC_METHODS

    public void InitModuleHandlers(KActions kActions)
    {
        this.kActions = kActions;

        kActions.OnScoreRecieved += UpdateScore;
        kActions.OnCrazinessUpdated += UpdateCraziness;
    }

    public void DeInitModuleHandlers()
    {
        kActions.OnScoreRecieved -= UpdateScore;
        kActions.OnCrazinessUpdated -= UpdateCraziness;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score " + score;
    }

    public void UpdateCraziness(float crazinessBase, float crazinessPoints)
    {
        crazinessBar.fillAmount = crazinessPoints / crazinessBase;
        CrazinessIcon.transform.position =
            Vector3.Lerp(barStart.position, barEnd.position, crazinessPoints / crazinessBase);
    }

    #endregion
}
