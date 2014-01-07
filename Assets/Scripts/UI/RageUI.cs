using UnityEngine;
using System.Collections;

public class RageUI : MonoBehaviour
{
    public Texture rage0;
    public Texture rage1;
    public Texture rage2;
    public Texture rage3;
    public Texture rage4;
    public Texture rage5;
    public Texture rage6;
    public Texture rage7;
    public Texture rage8;
    public Texture rage9;
    public Texture rage10;
    
    private Texture[] rageArray;
    private GUITexture gui;
    private int rageAmount = 0;
    
    void Start()
    {
        gui = GetComponentInChildren<GUITexture>();
        rageArray = new Texture[] {rage0, rage1, rage2, rage3, rage4, rage5, rage6, rage7, rage8, rage9, rage10};
    }
    
    public void AddRage(int amount)
    {
        rageAmount = Mathf.Min(10, rageAmount + amount);
        gui.texture = rageArray[rageAmount];
    }
    
    // if amount is new, then swap textures
    public void SetRage(int amount)
    {
        if (rageAmount != amount) {
            rageAmount = amount;
            gui.texture = rageArray[rageAmount];
        }
    }
    
    public void Reset()
    {
        rageAmount = 0;
        gui.texture = rageArray[0];
    }
}
