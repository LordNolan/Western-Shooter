using UnityEngine;
using System.Collections;

public class RageBehavior : MonoBehaviour
{
    private int currentRage;
    private float rageTime = 10.0f;
    private float currentRageTime;
    private bool enraged = false;
    
    void Update()
    {
        if (enraged) {
            // TODO: enrage player
        }
    }
    
    public void AddRage(int amount)
    {
        currentRage = Mathf.Min(10, currentRage + amount);
        if (currentRage == 10)
            enraged = true;
    }
}