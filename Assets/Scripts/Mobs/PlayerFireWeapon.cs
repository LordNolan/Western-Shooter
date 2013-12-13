using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public float heightOffset = 0.3f;
    public float directionOffset = 0.3f;
    public int ammoAmount = 20;
    public float fireDelayTime;
    
    public AudioClip fireBullet;
    public AudioClip fireEmpty;
    
    float currentTime;
    
    void Start()
    {
        // ensures we can fire as soon as game starts
        currentTime = fireDelayTime;
    }
    
    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > fireDelayTime && !GlobalParams.InNonPlayingState() && Input.GetMouseButton(0)) { // left click or held down
            currentTime = 0; // reset delay            
           
            if (ammoAmount <= 0) {
                transform.FindChild("Pistol").GetComponent<PistolAnimation>().FireEmpty(); // fire animation
                audio.PlayOneShot(fireEmpty);
            }
            else {
                transform.FindChild("Pistol").GetComponent<PistolAnimation>().Fire(); // fire animation
                audio.PlayOneShot(fireBullet);
                transform.FindChild("Pistol").GetComponent<RaycastFire>().Fire(GetBulletSpawnPosition(), GetForwardDirection()); // fire raycast
            }
                
            GameObject.FindWithTag("UI").BroadcastMessage("AmmoSpent", GetAmmoAmount());
        }
    }
    
    int GetAmmoAmount()
    {
        return Mathf.Max(0, --ammoAmount);
    }
    
    Vector3 GetBulletSpawnPosition()
    {
        // our position + offset for camera height
        return transform.position + Vector3.up * heightOffset;
    }
    
    Vector3 GetForwardDirection()
    {
        // our forward + offset for camera height
        return transform.forward + Vector3.up * directionOffset;
    }
}
