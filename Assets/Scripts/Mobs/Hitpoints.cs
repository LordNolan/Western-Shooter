using UnityEngine;
using System.Collections;

public class Hitpoints : MonoBehaviour
{
    public int startingHP;
    public Sprite deadSprite;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public bool mobDead = false;
    bool flicker = false;
    int HP;
    float flickerTime;
    public float maxFlickerTime;

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
    
    public void Heal(int amount)
    {
        HP = Mathf.Min(HP + amount, startingHP);
        GameObject.FindWithTag("UI").BroadcastMessage("SetPlayerHitpoints", HP);
    }
    
    public void TakeDamage(int amount)
    {
        if (CompareTag("Player") && !GlobalParams.IsPlayerEnraged()) {
            if ((HP -= amount) <= 0)
                PlayerDied();
            else {
                audio.PlayOneShot(hurtSound);
                Camera.main.GetComponent<PixelizeOnHit>().Hit();
            }
            GameObject.FindWithTag("UI").BroadcastMessage("SetPlayerHitpoints", HP);
        } else if (!CompareTag("Player")) {
            if ((HP -= amount) <= 0)
                MobDied();
            else
                flicker = true; // enemy flicker

            audio.PlayOneShot(hurtSound); // enemy hit sound
        }
    }
    
    public bool IsHurt()
    {
        return HP != startingHP;
    }
    
    void Update()
    {
        MobSpriteFlicker();
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
        
        // award points
        GetComponent<ScoreValue>().AwardPoints();
        
        // attempt to drop loot
        LootDrop ld = GetComponent<LootDrop>();
        if (ld != null)
            ld.DoDropLoot();
    }
    
    void MobSpriteFlicker()
    {
        if (flicker) {
            GetComponent<SpriteRenderer>().color = Color.red;
            if ((flickerTime += Time.deltaTime) > maxFlickerTime) {
                GetComponent<SpriteRenderer>().color = Color.white;
                flicker = false;
                flickerTime = 0;
            }
            
        }
    }
}
