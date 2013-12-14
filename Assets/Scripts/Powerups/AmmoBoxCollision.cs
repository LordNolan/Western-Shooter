using UnityEngine;
using System.Collections;

public class AmmoBoxCollision : MonoBehaviour 
{
    public int amount;
    
	void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerFireWeapon>().AddAmmo(amount);
            Destroy(gameObject);
        }
    }
}
