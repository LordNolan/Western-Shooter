using UnityEngine;
using System.Collections;

public class Hitpoints : MonoBehaviour
{
    public int startingHP;
    public Sprite deadSprite;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public bool mobDead = false;
    
    int HP;
    
    void Start()
    {
        // default behavior
        HP = startingHP;
        
        // if we're player and we won last level, lets set our hp to that amount
        if (CompareTag("Player")) {
            HP = startingHP;
            GameObject.FindWithTag("UI").BroadcastMessage("SetPlayerHitpoints", HP);
        }  
    }
    
    void TakeDamage(int amount)
    {
        if (CompareTag("Player")) {
            GameObject.FindWithTag("UI").BroadcastMessage("PlayerHit", amount);
            if ((HP -= amount) <= 0)
                PlayerDied();
            else
                audio.PlayOneShot(hurtSound);
        } else {
            if ((HP -= amount) <= 0)
                MobDied();
            else
                audio.PlayOneShot(hurtSound);
        }
    }
    
    void PlayerDied()
    {
        GameObject.FindWithTag("Global").SendMessage("PlayerDied");
    }
    
    void MobDied()
    {
        audio.PlayOneShot(deathSound);
        GameObject.FindWithTag("Global").SendMessage("MobDied");
        GetComponent<SpriteRenderer>().sprite = deadSprite; // set it to dead sprite
        collider.enabled = false; // turn off collider
        mobDead = true;
    }
}
