using UnityEngine;
using System.Collections;

public class GlobalParams
{
    static bool worldGenComplete = false;
    static bool mobAIDelayComplete = false;
    static bool inNonPlayingState = false;
    
    public static float fireXRotationOffset = 10.0f;
    
    void Start()
    {
        Screen.lockCursor = true; // lock the mouse
    }
    
    public static float GetFireXRotationOffset()
    {
        return fireXRotationOffset;
    }
    
    public static void EnterNonPlayingState()
    {
        inNonPlayingState = true;
    }
    
    public static bool InNonPlayingState()
    {
        return inNonPlayingState;
    }
    
    public static void ResetForNewLevel()
    {
        worldGenComplete = false;
        mobAIDelayComplete = false;
        inNonPlayingState = false;
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