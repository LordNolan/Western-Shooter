using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public float heightOffset = 0.3f;
    public float directionOffset = 0.3f;
    public float fireDelayTime;
    
    public AudioSource fireBullet;
    public AudioSource fireEmpty;
    
    Transform pistol1;
    Transform pistol2;
    bool isPistol1TurnToShoot = true;
    float currentTime;
    
    void Start()
    {
        // ensures we can fire as soon as game starts
        currentTime = fireDelayTime;
        pistol1 = transform.FindChild("Pistol1");
        pistol2 = transform.FindChild("Pistol2");
    }
    
    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > fireDelayTime && !GlobalParams.InNonPlayingState() && Input.GetMouseButton(0)) { // left click or held down
            currentTime = 0; // reset delay            
            
            Transform pistol = (isPistol1TurnToShoot) ? pistol1 : pistol2; // get correct pistol to fire
            isPistol1TurnToShoot = !isPistol1TurnToShoot;
            
            pistol.GetComponent<PistolAnimation>().Fire(); // fire animation
            fireBullet.Play(); // fire noise
            pistol.GetComponent<RaycastFire>().Fire(GetBulletSpawnPosition(), GetForwardDirection()); // fire raycast
            GetComponent<PlayerMovement>().shouldRecoil = true;
        }
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
