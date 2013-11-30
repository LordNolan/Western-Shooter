using UnityEngine;
using System.Collections.Generic;

public class HitpointsUI : MonoBehaviour
{
    int hitpoints;
    List<Transform> badgeList;
    
    void Awake()
    {
        badgeList = new List<Transform>();
        foreach (Transform child in transform) {
            badgeList.Add(child);
        }
    }
    
    public void SetPlayerHitpoints(int amount)
    {
        hitpoints = amount;
        // guiText.text = "HP: " + hitpoints;
    }
    
    public void PlayerHit(int amount)
    {
        hitpoints = Mathf.Max(0, hitpoints - amount);
        badgeList[0].GetComponent<HPBadgeUI>().Hit();
        // guiText.text = "HP: " + hitpoints;
    }
}
