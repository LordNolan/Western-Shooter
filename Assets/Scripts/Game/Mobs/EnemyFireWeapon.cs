using UnityEngine;
using System.Collections;

public class EnemyFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private double currentTime = 0;
    private double delay = 1.0;

    void Update()
    {
        if (GlobalParams.IsMobAIDelayComplete()) {
            GameObject player = (GameObject) GameObject.FindGameObjectWithTag("Player");
            
            // fire at the player
            if (player != null && (currentTime += Time.deltaTime) >= delay) {
                currentTime = 0;
                SetBulletSpeed();
                // create bullet rotated to point at player (not camera)
                float x = bulletPrefab.transform.localEulerAngles.x;
                Vector3 relativePos = player.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(x, rotation.eulerAngles.y, 0));
            }
        }
    }

    void SetBulletSpeed()
    {
        bulletPrefab.GetComponent<BulletMovement>().speed = bulletSpeed;
    }
}
