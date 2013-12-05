using UnityEngine;
using System.Collections.Generic;

public class PowerupController : MonoBehaviour 
{
    /*** POWERUPS ***/
    public GameObject doubleDamage;
    public GameObject speedBuff;
    /*** POWERUPS ***/
    
    HashSet<string> activePowerups;
    List<GameObject> powerups;
    
	void Start () 
    {
        activePowerups = new HashSet<string>();
        powerups = new List<GameObject>();
        
        // add POWERUPS to list
        powerups.Add(doubleDamage);
        powerups.Add(speedBuff);
	}
    
    // returns child object model for chest and sends other child object icon to UI
    public GameObject AddRandomPowerup()
    {
        GameObject choice = powerups[Random.Range(0, powerups.Count)];
        activePowerups.Add(choice.name);
        ActivatePowerup(choice.name);
        GameObject[] children = GetChildrenArrayFromPowerup(choice);
        GameObject.FindGameObjectWithTag("UI").BroadcastMessage("AddPower", children[1]); // to UI
        return children[0]; // to chest
    }
    
    // 0 - the model that pops out of box
    // 1 - the icon that we put in corner
    GameObject[] GetChildrenArrayFromPowerup(GameObject powerup)
    {
        GameObject[] go = new GameObject[2];
        foreach(Transform child in powerup.transform)
        {
            if (child.gameObject.CompareTag("Model")) go[0] = child.gameObject;
            else if (child.gameObject.CompareTag("Icon")) go[1] = child.gameObject;
            else Debug.Log("[PowerupController.GetChildrenArray] tag of child doesn't match Model or Icon.");
        }
        return go;
    }
    
    public void Reset() 
    {
        activePowerups.Clear();
    }
    
    void ActivatePowerup(string name)
    {
        switch(name)
        {
            case "DoubleDamage":
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFireWeapon>().pu_DamageModifier *= 2;
                break;
            case "SpeedBuff":
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().pu_SpeedBoost += 2.0f;
                break;
            default:
                Debug.LogError("[PowerupController.ActivatePowerup] name not found.");
                break;
        }
    }
}
