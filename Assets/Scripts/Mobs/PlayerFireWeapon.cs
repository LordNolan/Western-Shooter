using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float heightOffset = 0.3f;
    public float forwardOffset = 0.3f;
    void Update()
    {
        if (!GlobalParams.InNonPlayingState() && Input.GetMouseButtonDown(0)) { // left click
            // create bullet rotated to match player's facing direction + rotation offset due to UI
            float x = transform.localEulerAngles.x + bulletPrefab.transform.localEulerAngles.x - GlobalParams.GetFireXRotationOffset();
            float y = transform.localEulerAngles.y;
            SetBulletSpeed();
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
}
