using UnityEngine;
using TMPro;

public class ButtonUI : MonoBehaviour
{
    Color originalColor = Color.white;
    Color pressedColor = Color.red;

    public void PointerEnter(TMP_Text text) 
    {
        text.color = pressedColor;
        PointerEnterSound();
    }

    public void PointerEnterSound()
    {
        AkSoundEngine.PostEvent("ui_button", gameObject);
    }

    public void PointerDown()
    {
        AkSoundEngine.PostEvent("ui_menu", gameObject);
    }

    public void PointerExit(TMP_Text text)
    {
        text.color = originalColor;
    }
}
