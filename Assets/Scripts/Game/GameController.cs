using UnityEngine;
using System.Collections;
using System.Text;

public class GameController : MonoBehaviour
{
    public GameState currentState;
    private bool isDead = false;
    public double mobAIdelay = 2.0;
    private double currentTimer = 0;
    public int mobCount = 0;
    
    // scoreboard vars
    private int mobsKilled = 0;
    private int roundsWon = 0;
    private float timePlayed = 0;
    
    public AudioSource winAudio;
    public AudioSource loseAudio;
    private bool winPlayed = false;
    private bool losePlayed = false;
    private bool wonPrevious = false;
    private int winningHP;
    
    public enum GameState
    {
        StartScreen,
        NewGame,
        Playing,
        PlayerDead,
        LevelWon
    }
    
    void Start()
    {
        Screen.lockCursor = true; // lock mouse cursor on screen
        currentState = GameState.StartScreen;
    }
	
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // keep mouse cursor locked when we come back
            Screen.lockCursor = true;
        
        // DEBUG: reset game on R
        if (Input.GetKeyDown(KeyCode.R)) {
            isDead = true;
            GlobalParams.EnterNonPlayingState();
        }
        
        // DEBUG: 10 rage on P
        if (Input.GetKeyDown(KeyCode.P)) {
            GameObject.FindWithTag("UI").BroadcastMessage("AddRage", 10);
            GameObject.FindWithTag("Player").GetComponent<RageBehavior>().AddRage(10);
        }
        
        // DEBUG: kill all enemies
        if (Input.GetKeyDown(KeyCode.K)) {
            foreach (Transform child in transform) {
                if (child.CompareTag("Enemy"))
                    Destroy(child.gameObject);
            }   
        }
        
        // quit the game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
             
        switch (currentState) {
            case GameState.StartScreen:
                NewGameProcess();
                break;
        // new game or new level because of win/death
            case GameState.NewGame:
                if (!GlobalParams.IsWorldGenStarted()) { // generate world if we haven't yet
                    GlobalParams.MarkWorldGenStarted();
                    SendMessage("GenerateWorld");
                }
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
                timePlayed += Time.deltaTime; // increment time counter
                break;
            case GameState.PlayerDead:
                PlayLoseAudio(); // play death sound
                //GameObject.FindWithTag("Global").GetComponent<FadeBackground>().MakeShaded(); // make background dark
                string sb = DisplayScoreboard();
                if (!loseAudio.isPlaying) {
                    GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", sb + "Press Spacebar to Restart");
                }
				// don't allow spacebar continue until audio is done playing.
                if (Input.GetButtonDown("Fire3") && !loseAudio.isPlaying) {
                    StartNewGameFromDead(); // start new game if spacebar
                }
                break;
            case GameState.LevelWon:
                PlayWinAudio(); // play win sound
                //GameObject.FindWithTag("Global").GetComponent<FadeBackground>().MakeShaded(); // make background dark
                GameObject.FindWithTag("Player").GetComponent<RageBehavior>().WinLevelEnragedCheck(); // if enraged on win, give them full rage
                GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", "Press Spacebar for Next Level"); // inform player of next level
                if (Input.GetButtonDown("Fire3"))
                    StartNewLevelFromWin(); // start new level if spacebar or 360 A
                break;
        }
    }
    
    string DisplayScoreboard()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("You were killed. RIP");
        sb.AppendLine("Mobs Killed:\t" + mobsKilled);
        sb.AppendLine("Rounds Won:\t" + roundsWon);
        sb.AppendLine("Time Played:\t" + GetTimePlayedString());
        GameObject.FindWithTag("UI").BroadcastMessage("SetMessage", sb.ToString());
        return sb.ToString();
    }
    
    string GetTimePlayedString()
    {
        if (timePlayed >= 60)
            return (int)(timePlayed / 60) + "m " + (int)(timePlayed % 60) + "s";
        else
            return (int)timePlayed + "s";
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
        roundsWon++;        // increment scoreboard counter
        NewGameProcess();
    }
    
    void StartNewGameFromDead()
    {
        losePlayed = false; // reset audio play
        isDead = false;     // reset player death flag
        GameObject.FindWithTag("UI").BroadcastMessage("Reset"); // resets powerups
        GetComponent<SpawnPlayer>().DestroyPlayer(); // destroy player
        ResetScoreboard();
        NewGameProcess();
    }
    
    void ResetScoreboard()
    {
        mobsKilled = 0; // reset mob kill count
        roundsWon = 0;  // reset round win count
        timePlayed = 0; // reset time played amount
    }
    
    void NewGameProcess()
    {
        //GameObject.FindWithTag("Global").GetComponent<FadeBackground>().MakeClear(); // clear faded background
        ClearMessageUI();
        GlobalParams.ResetForNewLevel();
        GameObject.Find("LoadingCamera").GetComponent<LoadingBehavior>().ShowLoadingScreen();
        currentState = GameState.NewGame;
    }
    
    public int GetMobCount()
    {
        return mobCount;
    }
    
    public void SetMobCount(int amount)
    {
        mobCount = amount;
    }
    
    public void ChestWin()
    {    
        currentState = GameState.LevelWon;
        GlobalParams.EnterNonPlayingState();
    }
    
    public void MobDied()
    {
        mobCount--;
        mobsKilled++;
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
