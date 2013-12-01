using UnityEngine;
using System.Collections;

public class EnemyFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public double firingRange = 4.0;
    private double currentTime = 0;
    public float delay = 1.0f;
    public float delayVariance = 0.2f;
    private double currentDelay;
    
    void Start()
    {
        currentDelay = GetNextFiringTime();
    }
    
    void Update()
    {
        if (GlobalParams.IsMobAIDelayComplete() && !GetComponent<Hitpoints>().mobDead) {
            GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");
            
            // fire at the player
            if (player != null && !GlobalParams.InNonPlayingState() && WithinFiringRange(player.transform.position) && (currentTime += Time.deltaTime) >= currentDelay) {
                currentTime = 0;
                SetBulletSpeed();
                // create bullet rotated to point at player (not camera)
                float x = bulletPrefab.transform.localEulerAngles.x;
                Vector3 relativePos = player.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(x, rotation.eulerAngles.y, 0));
                audio.Play();
            }
        }
    }
    
    double GetNextFiringTime()
    {
        return Random.Range(delay - delayVariance, delay + delayVariance);
    }
    
    bool WithinFiringRange(Vector3 player)
    {
        return (Vector3.Distance(player, transform.position) < firingRange);
    }
    
    void SetBulletSpeed()
    {
        bulletPrefab.GetComponent<BulletMovement>().speed = bulletSpeed;
    }
}
