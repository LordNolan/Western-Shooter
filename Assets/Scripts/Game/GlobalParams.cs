using UnityEngine;
using System.Collections;

public class GlobalParams
{
    static bool worldGenComplete = false;
    static bool worldGenStarted = false;
    static bool mobAIDelayComplete = false;
    static bool inNonPlayingState = false;
    static bool playerSpawned = false;
    static bool isEnraged = false;
    
    public static bool IsPlayerSpawned()
    {
        return playerSpawned;
    }
    
    public static bool IsPlayerEnraged()
    {
        return isEnraged;
    }
    
    public static void MarkPlayerEnraged()
    {
        isEnraged = true;
    }
    
    public static void ResetPlayerEnraged()
    {
        isEnraged = false;
    }
    
    public static void MarkPlayerSpawned()
    {
        playerSpawned = true;
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
        worldGenStarted = false;
        mobAIDelayComplete = false;
        inNonPlayingState = false;
        playerSpawned = false;
        isEnraged = false;
    }
    
    public static void MarkWorldGenStarted()
    {
        worldGenStarted = true;
    }
    
    public static bool IsWorldGenStarted()
    {
        return worldGenStarted;
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