using UnityEngine;
using System.Collections;

public class GlobalParams
{
    static bool worldGenComplete = false;
    static bool mobAIDelayComplete = false;
    
    void Start()
    {
        Screen.lockCursor = true; // lock the mouse
    }
    public static void MarkWorldGenComplete()
    {
        worldGenComplete = true;
    }
	
    public static bool IsWorldGenComplete()
    {
        return worldGenComplete;
    }
    
    public static void MarkMobAIDelayComplete()
    {
        mobAIDelayComplete = true;
    }
    
    public static bool IsMobAIDelayComplete()
    {
        return mobAIDelayComplete;
    }
}