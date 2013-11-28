using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    private bool isOpen = false;
    
    void Start()
    {
        animation.PlayQueued("Idle_Closed", QueueMode.CompleteOthers);
    }
    
    void PlayOpenAnimation()
    {
        animation.PlayQueued("Open", QueueMode.CompleteOthers);
        animation.PlayQueued("Idle_Open", QueueMode.CompleteOthers);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (!isOpen && collision.gameObject.CompareTag("Player")) {
            PlayOpenAnimation();
            isOpen = true;
        }
    }
}