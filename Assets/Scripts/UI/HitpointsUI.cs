using UnityEngine;
using System.Collections.Generic;

public class HitpointsUI : MonoBehaviour
{
    List<Transform> badgeList;
    int maxHP;
    
    void Awake()
    {
        badgeList = new List<Transform>();
        foreach (Transform child in transform) {
            if (child.CompareTag("Badge"))
                badgeList.Add(child);
        }
        
        maxHP = badgeList.Count;
    }
    
    public void SetPlayerHitpoints(int amount)
    {
        amount = Mathf.Max(0, amount);
        for (int i = 0; i < amount; i++) {
            badgeList[i].GetComponent<HPBadgeUI>().Heal();
        }
        
        for (int j = amount; j < maxHP; j++) {
            badgeList[j].GetComponent<HPBadgeUI>().Hit();
        }
    }
    
    public void PlayerHit(int hpLeft)
    {
        SetPlayerHitpoints(hpLeft);
    }
}
