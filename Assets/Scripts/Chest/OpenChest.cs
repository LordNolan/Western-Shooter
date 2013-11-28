using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    void Start()
    {
        animation.PlayQueued("Idle_Closed", QueueMode.CompleteOthers);
        animation.PlayQueued("Open", QueueMode.CompleteOthers);
        animation.PlayQueued("Idle_Open", QueueMode.CompleteOthers);
    }
}
