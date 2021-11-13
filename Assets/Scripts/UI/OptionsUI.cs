using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private Image musicImage = null;
    [SerializeField] private Image sfxImage = null;

    [SerializeField] private Transform start = null;
    [SerializeField] private Transform end = null;

    #endregion

    #region PUBLIC_METHODS

    public void ChangeMusic()
    {
        musicImage.fillAmount = GetPercentValue();
    }

    public void ChangeSfx()
    {
        sfxImage.fillAmount = GetPercentValue();
    }

    #endregion

    #region PRIVATE_METHODS

    private float GetPercentValue()
    {
        Vector3 mousePos = Input.mousePosition;
        return ((start.position.x - end.position.x) - (start.position.x - mousePos.x)) / 100;
    }

    #endregion
}
