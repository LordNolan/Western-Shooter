using UnityEngine;
using System.Collections;

// turns on/off the showing of element
public class HideUI : MonoBehaviour
{
    public void HideElement()
    {
        if (guiTexture != null)
            guiTexture.enabled = false;
            
        if (renderer != null)
            renderer.enabled = false;
    }
    
    public void ShowElement()
    {
        if (guiTexture != null)
            guiTexture.enabled = true;
            
        if (renderer != null)
            renderer.enabled = true;
    }
}
