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
    private Sounds sound = null;

    #endregion

    #region UNITY_CALLS

    private void Start()
    {
        InitSounds();
    }

    #endregion

    #region PUBLIC_METHODS

    public void ChangeMusic(bool increment)
    {
        musicImage.fillAmount = GetPercentValue(musicImage.fillAmount, increment);
        AkSoundEngine.SetRTPCValue("mx_volume", musicImage.fillAmount);
        sound.Music = musicImage.fillAmount;
    }

    public void ChangeSfx(bool increment)
    {
        sfxImage.fillAmount = GetPercentValue(sfxImage.fillAmount, increment);
        AkSoundEngine.SetRTPCValue("sfx_volume", sfxImage.fillAmount);
        sound.Sfx = sfxImage.fillAmount;
    }

    #endregion

    #region PRIVATE_METHODS

    private void InitSounds()
    {
        if (GameManager.Get().Sound == null)
        {
            GameManager.Get().InitSound(sfxImage.fillAmount, musicImage.fillAmount);
        }

        sound = GameManager.Get().Sound;
        musicImage.fillAmount = sound.Music;
        sfxImage.fillAmount = sound.Sfx;
    }

    private float GetPercentValue(float soundBase, bool increment)
    {
        return increment ? soundBase + soundSpeed : soundBase - soundSpeed;
    }

    #endregion
}
