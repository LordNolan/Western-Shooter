using UnityEngine;
using System.Collections;

public class Hitpoints : MonoBehaviour
{
    public int startingHP;
    public int HP { get; private set; }
    
    // Use this for initialization
    void Start()
    {
        HP = startingHP;
        if (CompareTag("Player"))
            GameObject.Find("UI").BroadcastMessage("SetPlayerHitpoints", startingHP);
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
