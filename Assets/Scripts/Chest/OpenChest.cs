using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    private bool isOpen = false;
    
    void Start()
    {
        animation.PlayQueued("Idle_Closed", QueueMode.CompleteOthers);
    }
    
    void PlayOpenAnimation(GameObject powerup)
    {
        animation.PlayQueued("Open", QueueMode.CompleteOthers);
        Instantiate(powerup, transform.position, transform.rotation);
        animation.PlayQueued("Idle_Open", QueueMode.CompleteOthers);
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (!isOpen && collider.CompareTag("Player")) {
            // Get random powerup so we can pop it up on lid open
            GameObject powerup = GameObject.FindGameObjectWithTag("Global").GetComponent<PowerupController>().AddRandomPowerup();
            PlayOpenAnimation(powerup);
            isOpen = true;
        }
    }
}