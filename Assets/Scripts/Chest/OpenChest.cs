using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    public GameObject powerup;
    private bool isOpen = false;
    
    void Start()
    {
        animation.PlayQueued("Idle_Closed", QueueMode.CompleteOthers);
    }
    
    void PlayOpenAnimation()
    {
        animation.PlayQueued("Open", QueueMode.CompleteOthers);
        Instantiate(powerup, transform.position, transform.rotation);
        animation.PlayQueued("Idle_Open", QueueMode.CompleteOthers);
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (!isOpen && collider.CompareTag("Player")) {
            PlayOpenAnimation();
            isOpen = true;
        }
    }
}