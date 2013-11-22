using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    GameState currentState;
    
    enum GameState
    {
        PromptStart,
        Playing,
        PlayerDead
    }
    
    void Start()
    {
        currentState = GameState.PromptStart;
    }
	
    void Update()
    {
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
        currentState = GameState.PlayerDead;
        Debug.Log("death");
    }
}
