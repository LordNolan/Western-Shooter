using UnityEngine;
using System.Collections;

public class EnemyFireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    private double currentTime = 0;
    private double delay = 1.0;

    void Update()
    {
        GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");

        if (player != null && (currentTime += Time.deltaTime) >= delay)
        {
            currentTime = 0;
            // create bullet rotated to point at player (not camera)
            float x = bulletPrefab.transform.localEulerAngles.x;

            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(x, rotation.eulerAngles.y, 0));
        }
    }
}
