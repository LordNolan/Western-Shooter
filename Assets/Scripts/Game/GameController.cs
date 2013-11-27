﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameState currentState;
    private bool isDead = false;
    public double mobAIdelay = 2.0;
    private double currentTimer = 0;
    public int mobCount = 0;
    private int mobsKilled = 0;
    
    public AudioSource winAudio;
    public AudioSource loseAudio;
    private bool winPlayed = false;
    private bool losePlayed = false;
    private bool wonPrevious = false;
    private int winningHP;
    
    public enum GameState
    {
        NewGame,
        Playing,
        PlayerDead,
        LevelWon
    }
    
    public int GetWinningHP()
    {
        if (wonPrevious) {
            wonPrevious = false;
            return winningHP;
        }
        return -1;
    }
    
    void Start()
    {
        Screen.lockCursor = true;
        currentState = GameState.NewGame;
    }
	
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Screen.lockCursor = true;
            
        switch (currentState) {
        case GameState.NewGame:
            currentTimer = 0;
            if (GlobalParams.IsWorldGenComplete()) {
                GameObject.Find("UI").BroadcastMessage("SetMessage", "");
                if (GetComponent<SpawnPlayerSetup>().DidPlayerSpawn()) {
                    currentState = GameState.Playing;
                }
            }
            break;
        case GameState.Playing:
            if ((currentTimer += Time.deltaTime) > mobAIdelay)
                GlobalParams.MarkMobAIDelayComplete();
            break;
        case GameState.PlayerDead:
            PlayLoseAudio();
            GameObject.Find("Environment").GetComponent<FadeBackground>().MakeShaded();
            GameObject.Find("UI").BroadcastMessage("SetMessage", "You Killed " + mobsKilled + " Bandits!\nPress Spacebar to Restart");
            if (Input.GetKeyDown(KeyCode.Space))
                StartNewGameFromDead();
            break;
        case GameState.LevelWon:
            PlayWinAudio(); 
            wonPrevious = true;
            GameObject.Find("Environment").GetComponent<FadeBackground>().MakeShaded();
            GameObject.Find("UI").BroadcastMessage("SetMessage", "Press Spacebar for Next Level");
            if (Input.GetKeyDown(KeyCode.Space))
                StartNewLevelFromWin();
            break;
        }
    }
    
    void PlayLoseAudio()
    {
        if (!losePlayed) {
            losePlayed = true;
            loseAudio.Play();
        }
    }
    
    void PlayWinAudio()
    {
        if (!winPlayed) {
            winPlayed = true;
            winAudio.Play();
        }
    }
    
    void StartNewLevelFromWin()
    {
        winPlayed = false;
        wonPrevious = true;
        winningHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Hitpoints>().HP;
        GameObject.Find("Environment").GetComponent<FadeBackground>().MakeClear();
        GameObject.Find("UI").BroadcastMessage("SetMessage", "");
        GlobalParams.ResetForNewLevel();
        GetComponent<SpawnPlayerSetup>().DestroyPlayer();
        GameObject.Find("Camera").GetComponent<MouseAimCamera>().ResetCamera();
        currentState = GameState.NewGame;
        SendMessage("GenerateWorld");
    }
    
    void StartNewGameFromDead()
    {
        losePlayed = false;
        mobsKilled = 0;
        GameObject.Find("Environment").GetComponent<FadeBackground>().MakeClear();
        GameObject.Find("UI").BroadcastMessage("SetMessage", "");
        GlobalParams.ResetForNewLevel();
        GetComponent<SpawnPlayerSetup>().DestroyPlayer();
        GameObject.Find("Camera").GetComponent<MouseAimCamera>().ResetCamera();
        currentState = GameState.NewGame;
        SendMessage("GenerateWorld");
        isDead = false;
    }
    
    public int GetMobCount()
    {
        return mobCount;
    }
    
    public void SetMobCount(int amount)
    {
        mobCount = amount;
    }
    
    public void MobDied()
    {
        mobCount--;
        mobsKilled++;
        if (mobCount <= 0) {
            currentState = GameState.LevelWon;
            GlobalParams.EnterNonPlayingState();
        }
    }
    
    public void PlayerDied()
    {
        if (!isDead) {
            isDead = true;
            currentState = GameState.PlayerDead;
            GlobalParams.EnterNonPlayingState();
        }
    }
}