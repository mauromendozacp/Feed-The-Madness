using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    public void PointerEnter() 
    {
        AkSoundEngine.PostEvent("ui_button", gameObject);
    }

    public void PointerDown()
    {
        AkSoundEngine.PostEvent("ui_menu", gameObject);
    }
}
