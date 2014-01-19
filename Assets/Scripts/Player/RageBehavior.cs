using UnityEngine;
using System.Collections;

public class RageBehavior : MonoBehaviour
{
    public AudioSource enrageSound;
    
    private int currentRage = 0;
    private float rageTime = 10.0f;
    private bool enraged = false;
    private PlayerMovement movement;
    private PlayerFireWeapon fireWeapon;
    
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        fireWeapon = GetComponent<PlayerFireWeapon>();
    }
    
    void Update()
    {
        // if enraged, tick down for 10 seconds
        if (enraged) {
            // keep currentRage at ceiling of 
            currentRage = Mathf.CeilToInt(rageTime -= Time.deltaTime);
            GameObject.FindWithTag("UI").BroadcastMessage("SetRage", currentRage);
        }
        
        if (rageTime <= 0) {
            StopEnrage();
            rageTime = 10.0f;
        }
    }
    
    // reduce fire delay by 33% and double move speed
    void StartEnrage()
    {
        enraged = true;
        enrageSound.Play();
        fireWeapon.fireDelayTime = .2f;
        movement.movementSpeed = 250.0f;
        GlobalParams.MarkPlayerEnraged();
        GameObject.FindWithTag("UI").BroadcastMessage("PlayerEnraged");
    }
    
    // undo rage bonuses
    void StopEnrage()
    {
        enraged = false;
        fireWeapon.fireDelayTime = .3f;
        movement.ResetSpeed();
        GlobalParams.ResetPlayerEnraged();
        GameObject.FindWithTag("UI").BroadcastMessage("PlayerStopEnraged");
    }
    
    // add rage on pickup
    public void AddRage(int amount)
    {
        currentRage = Mathf.Min(10, currentRage + amount);
        if (enraged)
            rageTime = Mathf.Min(10.0f, rageTime + amount);
        if (!enraged && currentRage == 10)
            StartEnrage();
    }
}