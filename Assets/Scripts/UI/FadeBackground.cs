using UnityEngine;
using System.Collections;

public class FadeBackground : MonoBehaviour
{
    public float shadeAmount = 0.23f;
    public Texture blackTexture;
    
    void Awake()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        //GUI.color = new Color(0, 0, 0, shadeAmount);
        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
        //MakeClear();
    }
   
    public void MakeClear()
    {
        GUI.color = new Color(0, 0, 0, shadeAmount);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
    }
    
    
    public void MakeShaded()
    {
        GUI.color = new Color(0, 0, 0, shadeAmount);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
    }
}
