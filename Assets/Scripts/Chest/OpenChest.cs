using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    bool isOpen = false;
    
    void Start()
    {
        animation.PlayQueued("Idle_Closed", QueueMode.CompleteOthers);
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (!isOpen && collider.CompareTag("Player")) {
            animation.PlayQueued("Open", QueueMode.CompleteOthers);
            animation.PlayQueued("Idle_Open", QueueMode.CompleteOthers);
            audio.Play();
            isOpen = true;
            GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>().ChestWin();
        }
    }
}