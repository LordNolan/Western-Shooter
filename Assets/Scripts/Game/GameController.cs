using UnityEngine;
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
    
    void Start()
    {
        Screen.lockCursor = true; // lock mouse cursor on screen
        currentState = GameState.NewGame;
    }
	
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // keep mouse cursor locked when we come back
            Screen.lockCursor = true;
            
        switch (currentState) {
        // new game or new level because of win/death
        case GameState.NewGame:
            if (GlobalParams.IsWorldGenComplete()) {
                ClearMessageUI();
                if (GetComponent<SpawnPlayerSetup>().DidPlayerSpawn()) { // player spawned, let's play
                    currentState = GameState.Playing;
                    currentTimer = 0; // timer for mob delay before kicking off AI
                }
            }
            break;
        case GameState.Playing:
            if ((currentTimer += Time.deltaTime) > mobAIdelay) // wait for delay before kicking off AI
                GlobalParams.MarkMobAIDelayComplete();
            break;
        case GameState.PlayerDead:
            PlayLoseAudio(); // play death sound
            GameObject.Find("Environment").GetComponent<FadeBackground>().MakeShaded(); // make background dark
            GameObject.Find("UI").BroadcastMessage("SetMessage", "You Killed " + mobsKilled + " Bandits!\nPress Spacebar to Restart"); // inform player of kill count
            if (Input.GetKeyDown(KeyCode.Space))
                StartNewGameFromDead(); // start new game if spacebar
            break;
        case GameState.LevelWon:
            PlayWinAudio(); // play win sound
            wonPrevious = true; // bool for carrying over previous level values
            GameObject.Find("Environment").GetComponent<FadeBackground>().MakeShaded(); // make background dark
            GameObject.Find("UI").BroadcastMessage("SetMessage", "Press Spacebar for Next Level"); // inform player of next level
            if (Input.GetKeyDown(KeyCode.Space))
                StartNewLevelFromWin(); // start new level if spacebar
            break;
        }
    }
    
    void ClearMessageUI()
    {
        GameObject.Find("UI").BroadcastMessage("SetMessage", "");
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
        winPlayed = false;  // reset audio play
        wonPrevious = true; // set flag for carrying over values
        winningHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Hitpoints>().HP; // keep previous HP
        NewGameProcess();
    }
    
    void StartNewGameFromDead()
    {
        losePlayed = false; // reset audio play
        mobsKilled = 0;     // reset mob kill count
        isDead = false;     // reset player death flag
        NewGameProcess();
    }
    
    void NewGameProcess()
    {
        GameObject.Find("Environment").GetComponent<FadeBackground>().MakeClear(); // clear faded background
        ClearMessageUI();
        GlobalParams.ResetForNewLevel();
        GetComponent<SpawnPlayerSetup>().DestroyPlayer();
        currentState = GameState.NewGame;
        SendMessage("GenerateWorld");
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
    
    public int GetWinningHP()
    {
        if (wonPrevious) {
            wonPrevious = false;
            return winningHP;
        }
        return -1;
    }
}
