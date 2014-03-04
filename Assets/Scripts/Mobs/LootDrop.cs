using UnityEngine;
using System.Collections;

public class LootDrop : MonoBehaviour
{
    public int lootDropChance;    
    public GameObject lootDrop;
    public GameObject hpDrop;
    
    public void DoDropLoot()
    {
        GameObject drop;
        if (Random.Range(0, 100) < lootDropChance) {
            // if player hurt, 50% chance to drop hp kit
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Hitpoints>().IsHurt()) {
                drop = (Random.Range(0, 2) == 1) ? lootDrop : hpDrop;
            } else {
                drop = lootDrop;
            }
            
            GameObject loot = (GameObject)Instantiate(drop, GetDropLootPosition(), drop.transform.rotation);
            loot.transform.parent = gameObject.transform.parent;
        }
    }
    
    // drop loot in front of dead mob facing player and on ground
    Vector3 GetDropLootPosition()
    {
        Vector3 pos = transform.position + transform.forward * 0.3f;
        pos.y = lootDrop.transform.position.y;
        return pos;
    }
    
}
