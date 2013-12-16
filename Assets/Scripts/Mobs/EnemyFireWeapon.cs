using UnityEngine;
using System.Collections;

public class EnemyFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private double currentTime = 0;
    public float delay = 1.0f;
    public float delayVariance = 0.2f;
    private double currentDelay;
    public AudioSource gunshotSound;
    
    void Start()
    {
        currentDelay = GetNextFiringTime();
    }
    
    public void FireAtPlayer()
    {
        GameObject player = (GameObject)GameObject.FindWithTag("Player");
        
        // fire at the player
        if ((currentTime += Time.deltaTime) >= currentDelay) {
            currentTime = 0;
            SetBulletSpeed();
            // create bullet rotated to point at player (not camera)
            float x = bulletPrefab.transform.localEulerAngles.x;
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(x, rotation.eulerAngles.y, 0));
            gunshotSound.Play();
        }
        
    }
    
    double GetNextFiringTime()
    {
        return Random.Range(delay - delayVariance, delay + delayVariance);
    }
    
    void SetBulletSpeed()
    {
        bulletPrefab.GetComponent<BulletMovement>().speed = bulletSpeed;
    }
}
