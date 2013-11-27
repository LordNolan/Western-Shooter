﻿using UnityEngine;
using System.Collections;

public class PlayerFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    void Update()
    {
        if (!GlobalParams.IsPlayerDead() && Input.GetMouseButtonDown(0)) { // left click
            // create bullet rotated to match player's facing direction
            float x = bulletPrefab.transform.localEulerAngles.x;
            float y = transform.localEulerAngles.y;
            SetBulletSpeed();
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(x, y, 0));
            audio.Play();
        }
    }

    void SetBulletSpeed()
    {
        bulletPrefab.GetComponent<BulletMovement>().speed = bulletSpeed;
    }
}
