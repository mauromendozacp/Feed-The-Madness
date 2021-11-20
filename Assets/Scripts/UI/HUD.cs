using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private TMP_Text scoreText = null;

    [Header("Craziness Bar"), Space]
    [SerializeField] private Image crazinessBar = null;
    [SerializeField] private GameObject CrazinessIcon = null;
    [SerializeField] private Transform barStart = null;
    [SerializeField] private Transform barEnd = null;

    [Header("Fade"), Space]
    [SerializeField] private CanvasGroup fadeCanvasGroup = null;
    [SerializeField] private float fadeTimer = 0f;

    #endregion

    #region PRIVATE_FIELDS

    private KActions kActions = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        StartCoroutine(StartFade());
    }

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

    #endregion

    #region PRIVATE_METHODS

    private void UpdateScore(int score)
    {
        scoreText.text = "Score " + score;
    }

    private void UpdateCraziness(float crazinessBase, float crazinessPoints)
    {
        crazinessBar.fillAmount = crazinessPoints / crazinessBase;
        CrazinessIcon.transform.position =
            Vector3.Lerp(barStart.position, barEnd.position, crazinessPoints / crazinessBase);
    }

    private IEnumerator StartFade()
    {
        float timer = 0f;
        while (timer < fadeTimer)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeTimer);

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    #endregion
}
