using UnityEngine;
using System.Collections;

public class AmmoBoxCollision : MonoBehaviour
{
    public AudioSource collectSound;
    bool collected = false;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) {
            if (CompareTag("Ammo")) {
                GameObject.FindWithTag("UI").BroadcastMessage("AddRage", 1);
                GameObject.FindWithTag("Player").GetComponent<RageBehavior>().AddRage(1);
            } else if (CompareTag("HPKit")) {
                GameObject.FindWithTag("Player").GetComponent<Hitpoints>().Heal(1);
            }
            
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
