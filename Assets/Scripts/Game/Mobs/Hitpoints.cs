using UnityEngine;
using System.Collections;

public class Hitpoints : MonoBehaviour
{
    public int startingHP;
    public int HP;
    
    void Start()
    {
        // default behavior
        HP = startingHP;
        
        // if we're player and we won last level, lets set our hp to that amount
        if (CompareTag("Player")) {
            int winningHP = GameObject.Find("Environment").GetComponent<GameController>().GetWinningHP();
            if (winningHP != -1)
                HP = winningHP;
            else
                HP = startingHP;
            GameObject.Find("UI").BroadcastMessage("SetPlayerHitpoints", HP);
        }  
    }
    
    void TakeDamage(int amount)
    {
        if (CompareTag("Player")) {
            GameObject.Find("UI").BroadcastMessage("PlayerHit", amount);
            if ((HP -= amount) <= 0)
                PlayerDied();
        } else {
            if ((HP -= amount) <= 0)
                MobDied();
        }
    }
    
    void PlayerDied()
    {
        GameObject.Find("Environment").SendMessage("PlayerDied");
    }
    
    void MobDied()
    {
        GameObject.Find("Environment").SendMessage("MobDied");
        Destroy(gameObject);
    }
    
    public void ResetHP()
    {
        Start();
    }
}
