using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

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
		HOTween.Init();
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
                    if (GlobalParams.IsPlayerSpawned()) { // player spawned, let's play
                        ClearMessageUI();
                        currentState = GameState.Playing;
                        currentTimer = 0; // timer for mob delay before kicking off AI
                    } else {
                        if (wonPrevious) {
                            GetComponent<SpawnPlayer>().ResetPlayerSpawn();
                            wonPrevious = false;
                        } else {
                            GetComponent<SpawnPlayer>().SpawnNewPlayer();
                        }
                    }
                }
                break;
            case GameState.Playing:
                if ((currentTimer += Time.deltaTime) > mobAIdelay) // wait for delay before kicking off AI
                    GlobalParams.MarkMobAIDelayComplete();
                break;
            case GameState.PlayerDead:
                PlayLoseAudio(); // play death sound
                GameObject.FindWithTag("Global").GetComponent<FadeBackground>().MakeShaded(); // make background dark
                GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", "You Killed " + mobsKilled + " Bandits!"); // inform player of kill count
				if (!loseAudio.isPlaying) {
				GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", "You Killed " + mobsKilled + " Bandits!\nPress Spacebar to Restart");
				}
				// don't allow spacebar continue until audio is done playing.
				if (Input.GetKeyDown(KeyCode.Space) && !loseAudio.isPlaying) {
                    StartNewGameFromDead(); // start new game if spacebar
				}
                break;
            case GameState.LevelWon:
                PlayWinAudio(); // play win sound
                GameObject.FindWithTag("Global").GetComponent<FadeBackground>().MakeShaded(); // make background dark
                GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", "Press Spacebar for Next Level"); // inform player of next level
                if (Input.GetKeyDown(KeyCode.Space))
                    StartNewLevelFromWin(); // start new level if spacebar
                break;
        }
    }
    
    void ClearMessageUI()
    {
        GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", "");
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
        NewGameProcess();
    }
    
    void StartNewGameFromDead()
    {
        losePlayed = false; // reset audio play
        mobsKilled = 0;     // reset mob kill count
        isDead = false;     // reset player death flag
        GameObject.FindWithTag("UI").BroadcastMessage("Reset");
        GetComponent<SpawnPlayer>().DestroyPlayer(); // destroy player
        NewGameProcess();
    }
    
    void NewGameProcess()
    {
        GameObject.FindWithTag("Global").GetComponent<FadeBackground>().MakeClear(); // clear faded background
        ClearMessageUI();
        GlobalParams.ResetForNewLevel();
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
}
