using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public float heightOffset = 0.3f;
    public float directionOffset = 0.3f;
    public int ammoAmount = 20;
    public float fireDelayTime;
    
    public AudioSource fireBullet;
    public AudioSource fireEmpty;
    
    float currentTime;
    
    void Start()
    {
        // ensures we can fire as soon as game starts
        currentTime = fireDelayTime;
        GameObject.FindWithTag("UI").BroadcastMessage("SetAmmoCount", ammoAmount);
    }
    
    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > fireDelayTime && !GlobalParams.InNonPlayingState() && Input.GetMouseButton(0)) { // left click or held down
            currentTime = 0; // reset delay            
           
            if (ammoAmount <= 0) {
                transform.FindChild("Pistol").GetComponent<PistolAnimation>().FireEmpty(); // out of ammo animation
                fireEmpty.Play();
            }
            else {
                transform.FindChild("Pistol").GetComponent<PistolAnimation>().Fire(); // fire animation
                fireBullet.Play();
                transform.FindChild("Pistol").GetComponent<RaycastFire>().Fire(GetBulletSpawnPosition(), GetForwardDirection()); // fire raycast
                GetComponent<PlayerMovement>().shouldRecoil = true;
            }
                
            GameObject.FindWithTag("UI").BroadcastMessage("SetAmmoCount", GetDecrementedAmmoAmount());
        }
    }
    
    int GetDecrementedAmmoAmount()
    {
        return Mathf.Max(0, --ammoAmount);
    }
    
    public void AddAmmo(int amount)
    {
        ammoAmount += amount;
        GameObject.FindWithTag("UI").BroadcastMessage("SetAmmoCount", ammoAmount);
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
