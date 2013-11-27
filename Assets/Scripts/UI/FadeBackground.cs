using UnityEngine;
using System.Collections;

public class FadeBackground : MonoBehaviour
{
    public float shadeAmount = 0.23f;
    
    void Awake()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
        MakeClear();
    }
   
    public void MakeClear()
    {
        guiTexture.color = Color.clear;
    }
    
    
    public void MakeShaded()
    {
        guiTexture.color = new Color(1, 1, 1, shadeAmount);
    }
}
