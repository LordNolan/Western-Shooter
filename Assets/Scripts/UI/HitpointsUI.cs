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
            if (child.CompareTag("Badge"))
                badgeList.Add(child);
        }
    }
    
    public void SetPlayerHitpoints(int amount)
    {
        hitpoints = amount;
        for (int x = 0; x < amount; x++) {
            badgeList[x].GetComponent<HPBadgeUI>().Heal();
        }
    }
    
    public void PlayerHit(int amount)
    {
        // when hit, we we need to remove badges equal to amount
        // we need to start removing from last alive badge and move forward. 
        for (int x = amount; x > 0; x--) {
            if (hitpoints > 0) {
                badgeList[hitpoints - 1].GetComponent<HPBadgeUI>().Hit();
                hitpoints--;
            }
        }
    }
}
