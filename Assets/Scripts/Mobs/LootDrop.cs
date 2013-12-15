using UnityEngine;
using System.Collections;

public class LootDrop : MonoBehaviour 
{
    public int lootDropChance;    
    public GameObject lootDrop;
    
	public void DoDropLoot()
    {
        if (Random.Range(0,100) < lootDropChance)
        {
            GameObject loot = (GameObject)Instantiate(lootDrop,GetDropLootPosition(),lootDrop.transform.rotation);
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
