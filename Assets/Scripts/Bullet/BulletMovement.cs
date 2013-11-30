using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour
{
    public float speed;
    public float selfDestructTime = 3.0f;
    float currentTime = 0;
    
    void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if ((currentTime += Time.deltaTime) > selfDestructTime) {
            Destroy(gameObject);
        }
    }
}
