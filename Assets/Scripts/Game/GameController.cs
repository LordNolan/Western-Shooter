﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    GameState currentState;
    bool isDead = false;
    
    enum GameState
    {
        PromptStart,
        Playing,
        PlayerDead
    }
    
    void Start()
    {
        Screen.lockCursor = true;
        currentState = GameState.PromptStart;
    }
	
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Screen.lockCursor = true;
            
        switch (currentState) {
        case GameState.PromptStart:
            break;
        case GameState.Playing:
            break;
        case GameState.PlayerDead:
            break;
        }
    }
    
    public void PlayerDied()
    {
        if (!isDead) {
            isDead = true;
            currentState = GameState.PlayerDead;
            GameObject.Find("UI").BroadcastMessage("SetMessage", "YOU ARE DEAD");
            audio.Play();
        }
    }
}
