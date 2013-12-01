using UnityEngine;
using System.Collections;

public class Hitpoints : MonoBehaviour
{
    public int startingHP;
    public int HP;
    
    public bool mobDead = false;
    
    public Sprite deadSprite;
    
    void Start()
    {
        // default behavior
        HP = startingHP;
        
        // if we're player and we won last level, lets set our hp to that amount
        if (CompareTag("Player")) {
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
        GetComponent<SpriteRenderer>().sprite = deadSprite; // set it to dead sprite
        collider.enabled = false; // turn off collider
        mobDead = true;
    }
    
    public void ResetHP()
    {
        Start();
    }
}
