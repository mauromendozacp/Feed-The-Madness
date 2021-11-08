using UnityEngine;
using TMPro;

public class ButtonUI : MonoBehaviour
{
    Color originalColor = Color.white;
    Color pressedColor = Color.red;

    public void PointerEnter(TMP_Text text) 
    {
        text.color = pressedColor;
        //AkSoundEngine.PostEvent("ui_button", gameObject);
    }

    public void PointerExit(TMP_Text text)
    {
        text.color = originalColor;
        //AkSoundEngine.PostEvent("ui_menu", gameObject);
    }
}
