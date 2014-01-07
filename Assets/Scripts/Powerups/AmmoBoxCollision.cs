using UnityEngine;
using System.Collections;

public class AmmoBoxCollision : MonoBehaviour
{
    public AudioSource collectSound;
    bool collected = false;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) {
            GameObject.FindWithTag("UI").BroadcastMessage("AddRage", 1);
            GameObject.FindWithTag("Player").GetComponent<RageBehavior>().AddRage(1);
            collectSound.Play();
            collected = true;
        }
    }
    
    void Update()
    {
        if (collected && !collectSound.isPlaying)
            Destroy(gameObject);
    }
}
