using UnityEngine;
using System.Collections;
using System;

public class ScoreUI : MonoBehaviour
{
    private GUITexture[] digitPositions;
    private GUITexture comma1;
    private GUITexture comma2;
    
    public Texture[] digitTextures;
    
    private int score = 0;
    
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
    
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreboard();
    }
    
    void UpdateScoreboard()
    {
        // set new score
        string temp = score.ToString();
        for (int i = 0; i < temp.Length; i++) {
            digitPositions[i].texture = digitTextures[(int)Char.GetNumericValue(temp[(temp.Length - 1) - i])];
        }
        
        // set commas if needed
        if (temp.Length >= 7) {
            comma1.enabled = false;
            comma2.enabled = false;
        } else if (temp.Length >= 4) {
            comma1.enabled = false;
        }
    }
    
    //TODO: instead of hiding commas, the texture for comma 
    // should be shown much like digits so that HideUI script works
    
    void Reset()
    {
        for (int i = 0; i < 9; i++) {
            digitPositions[i].texture = digitTextures[10]; // empty
        }
        digitPositions[0].texture = digitTextures[0];
        comma1.enabled = true;
        comma2.enabled = true;
        
        score = 0;
    }
}
