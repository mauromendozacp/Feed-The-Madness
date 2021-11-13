using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private PostProcessVolume volume = null;

    #endregion

    #region PRIVATE_METHODS

    private ChromaticAberration chromatic = null;
    private Vignette vignette = null;
    private float minChromaticValue = 0.2f;
    private float maxChromaticValue = 1f;
    private float minVignetteValue = 0f;
    private float maxVignetteValue = 1f;

    #endregion

    #region PUBLIC_METHODS

    public void Init()
    {
        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out vignette);
    }

    public void ChangeChromatic(float craziness, float crazinessBase)
    {
        float crazinessPercent = 100 - craziness * 100 / crazinessBase;
        float chromaticValue = (maxChromaticValue - minChromaticValue) * crazinessPercent / 100 + minChromaticValue;

        chromatic.intensity.value = chromaticValue;
    }

    public void ChangeVignette(bool increase, float timer)
    {
        vignette.intensity.value = Mathf.Lerp(
            increase ? minVignetteValue : maxVignetteValue,
            increase ? maxVignetteValue : minVignetteValue, 
            timer);
    }

    #endregion
}
