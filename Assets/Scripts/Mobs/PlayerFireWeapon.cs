using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float heightOffset = 0.3f;
    public float forwardOffset = 0.3f;
    public float FiringXRotationOffset = -10.0f;
    
    public int pu_DamageModifier = 1;  // variable for double damage
    
    public float fireDelayTime;
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
            // create bullet rotated to match player's facing direction + rotation offset due to UI
            float x = transform.localEulerAngles.x + bulletPrefab.transform.localEulerAngles.x + FiringXRotationOffset;
            float y = transform.localEulerAngles.y;
            SetBulletSpeed();
            SetBulletDamage();
            Instantiate(bulletPrefab, GetBulletSpawnPosition(), Quaternion.Euler(x, y, 0));
            audio.Play();
        }
    }
    
    Vector3 GetBulletSpawnPosition()
    {
        // our position + offset for camera height + offset for foward
        return transform.position + Vector3.up * heightOffset + transform.forward * forwardOffset;
    }

    void SetBulletSpeed()
    {
        bulletPrefab.GetComponent<BulletMovement>().speed = bulletSpeed;
    }
    
    void SetBulletDamage()
    {
        bulletPrefab.GetComponent<BulletCollision>().damageModifier = pu_DamageModifier;
    }
}
