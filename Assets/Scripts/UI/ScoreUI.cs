using UnityEngine;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    private GUITexture[] digitPositions;
    private GUITexture comma1;
    private GUITexture comma2;
    
    public Texture[] digitTextures;
    
    void Start()
    {
        digitPositions = new GUITexture[9];
        for (int i = 0; i < 9; i++) {
            digitPositions[i] = transform.FindChild("n" + i).guiTexture;
        }
        comma1 = transform.FindChild("comma1").guiTexture;
        comma2 = transform.FindChild("comma2").guiTexture;
        
        Reset();
    }
	
    void Update()
    {
        
    }
    
    void Reset()
    {
        for (int i = 0; i < 9; i++) {
            digitPositions[i].texture = digitTextures[10]; // empty
        }
        digitPositions[0].texture = digitTextures[0];
        comma1.enabled = true;
        comma2.enabled = true;
    }
}
