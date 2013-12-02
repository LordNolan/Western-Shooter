using UnityEngine;
using System.Collections.Generic;

public class PowerupsUI : MonoBehaviour {

	public Texture2D itemIcon1;
    
    List<Texture2D> activePowerups;
    
    void Start() {
        activePowerups = new List<Texture2D>();
    }
    
    public void Reset() {
        activePowerups.Clear();
    }
    
    public void AddPower() {
        activePowerups.Add(itemIcon1);
        
    }
}
