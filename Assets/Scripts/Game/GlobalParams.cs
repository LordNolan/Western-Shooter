using UnityEngine;
using System.Collections;

public class GlobalParams
{
    static bool worldGenComplete = false;
    static bool mobAIDelayComplete = false;
    static bool inNonPlayingState = false;
    static bool playerSpawned = false;
    
    public static bool IsPlayerSpawned()
    {
        return playerSpawned;
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
        mobAIDelayComplete = false;
        inNonPlayingState = false;
        playerSpawned = false;
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