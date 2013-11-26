using UnityEngine;
using System.Collections;

public class HitpointsUI : MonoBehaviour
{
    public int hitpoints;
    
    public void SetPlayerHitpoints(int amount)
    {
        hitpoints = amount;
        guiText.text = "HP: " + hitpoints;
    }
    
    public void PlayerHit(int amount)
    {
        hitpoints = Mathf.Max(0, hitpoints - amount);
        guiText.text = "HP: " + hitpoints;
    }
}
