using UnityEngine;
using TMPro;

public class ButtonUI : MonoBehaviour
{
    Color originalColor = Color.white;
    Color pressedColor = Color.red;

    public void PointerEnter(TMP_Text text) 
    {
        text.color = pressedColor;
    }

    public void PointerExit(TMP_Text text)
    {
        text.color = originalColor;
    }
}
