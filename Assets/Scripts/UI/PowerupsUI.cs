using UnityEngine;
using System.Collections;

public class PowerupsUI : MonoBehaviour
{	
    public float iconOffset = 0.035f;
    int actives = 0;
    
    public void Reset() 
    {
        actives = 0;
        // remove the powerup icons
        foreach (Transform child in gameObject.transform) {
            Destroy(child.gameObject);
        }
    }
    
    public void AddPower(GameObject power) 
    {
        GameObject icon = (GameObject) Instantiate(power, power.transform.position + new Vector3(actives * iconOffset,0,0), power.transform.rotation);
        icon.transform.parent = transform;
        actives++;
    }
}
