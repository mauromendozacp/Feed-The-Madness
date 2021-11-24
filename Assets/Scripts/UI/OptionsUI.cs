using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Image musicImage = null;
    [SerializeField] private Image sfxImage = null;

    #endregion

    #region PRIVATE_FIELDS

    private float soundSpeed = 0.1f;

    #endregion

    #region PUBLIC_METHODS

    public void ChangeMusic(bool increment)
    {
        musicImage.fillAmount = GetPercentValue(musicImage.fillAmount, increment);
        AkSoundEngine.SetRTPCValue("mx_volume", musicImage.fillAmount);
    }

    public void ChangeSfx(bool increment)
    {
        sfxImage.fillAmount = GetPercentValue(sfxImage.fillAmount, increment);
        AkSoundEngine.SetRTPCValue("sfx_volume", sfxImage.fillAmount);
    }

    #endregion

    #region PRIVATE_METHODS

    private float GetPercentValue(float soundBase, bool increment)
    {
        return increment ? soundBase + soundSpeed : soundBase - soundSpeed;
    }

    #endregion
}
