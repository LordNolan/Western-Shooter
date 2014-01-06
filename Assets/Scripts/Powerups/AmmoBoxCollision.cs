using UnityEngine;
using System.Collections;

public class AmmoBoxCollision : MonoBehaviour
{
    public int amount;
    public AudioSource collectSound;
    bool collected = false;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) {
            // TODO: change this to rage
            //collider.GetComponent<PlayerFireWeapon>().AddAmmo(amount);
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
