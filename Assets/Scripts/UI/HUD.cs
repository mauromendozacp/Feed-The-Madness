using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private TMP_Text scoreText = null;

    [Header("Throw Icon"), Space]
    [SerializeField] private Image throwImage = null;
    [SerializeField] private Image throwTimerImage = null;
    [SerializeField] private Sprite throwAvailable = null;
    [SerializeField] private Sprite throwUnavailable = null;
    [SerializeField] private Color throwStartColor = Color.white;
    [SerializeField] private Color throwEndColor = Color.white;

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
        kActions.OnThrowTimerUpdate += UpdateThrowTimer;
        kActions.OnChangeThrowIcon += ChangeThrowIcon;
    }

    public void DeInitModuleHandlers()
    {
        kActions.OnScoreRecieved -= UpdateScore;
        kActions.OnCrazinessUpdated -= UpdateCraziness;
        kActions.OnThrowTimerUpdate -= UpdateThrowTimer;
        kActions.OnChangeThrowIcon -= ChangeThrowIcon;
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

    private void ChangeThrowIcon(bool active)
    {
        throwImage.sprite = active ? throwAvailable : throwUnavailable;
    }

    private void UpdateThrowTimer(float throwTimer, float throwCooldown)
    {
        float interpole = throwTimer / throwCooldown;
        throwImage.fillAmount = interpole;
        throwTimerImage.fillAmount = interpole;
        throwTimerImage.color = Color.Lerp(throwStartColor, throwEndColor, interpole);
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
