using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameState currentState;
    private bool isDead = false;
    public double mobAIdelay = 2.0;
    private double currentTimer = 0;
    public int mobCount = 0;
    
    public AudioSource winAudio;
    public AudioSource loseAudio;
    
    public enum GameState
    {
        NewGame,
        Playing,
        PlayerDead,
        LevelWon
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
                currentState = GameState.Playing;
            }
            break;
        case GameState.Playing:
            if ((currentTimer += Time.deltaTime) > mobAIdelay)
                GlobalParams.MarkMobAIDelayComplete();
            break;
        case GameState.PlayerDead:
            GlobalParams.ResetForNewLevel();
            ResetPlayer();
            GetComponent<SpawnPlayerSetup>().ResetPlayerSpawn();
            GameObject.Find("Camera").GetComponent<MouseAimCamera>().ResetCamera();
            currentState = GameState.NewGame;
            loseAudio.Play();
            SendMessage("GenerateWorld");
            break;
        case GameState.LevelWon:
            GlobalParams.ResetForNewLevel();
            GetComponent<SpawnPlayerSetup>().ResetPlayerSpawn();
            GameObject.Find("Camera").GetComponent<MouseAimCamera>().ResetCamera();
            currentState = GameState.NewGame;
            winAudio.Play();
            SendMessage("GenerateWorld");
            break;
        }
        
    }
    
    void ResetPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Hitpoints>().ResetHP();
        isDead = false;
    }
    
    public int GetMobCount()
    {
        return mobCount;
    }
    
    public void SetMobCount(int amount)
    {
        mobCount = amount;
        GameObject.Find("UI").BroadcastMessage("SetEnemiesLeft", mobCount);
    }
    
    public void MobDied()
    {
        mobCount--;
        if (mobCount <= 0) {
            currentState = GameState.LevelWon;
        } else
            GameObject.Find("UI").BroadcastMessage("SetEnemiesLeft", mobCount);
    }
    
    public void PlayerDied()
    {
        if (!isDead) {
            isDead = true;
            currentState = GameState.PlayerDead;
            //GameObject.Find("UI").BroadcastMessage("SetMessage", "YOU ARE DEAD");
        }
    }
}
