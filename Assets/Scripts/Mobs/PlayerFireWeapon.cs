using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
	
    void Update() 
    {
        if (Input.GetMouseButtonDown(0)) { // left click
            // create bullet rotated to match player's facing direction
            float x = bulletPrefab.transform.localEulerAngles.x;
            float y = transform.localEulerAngles.y;
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(x, y, 0));
        }
    }
}
