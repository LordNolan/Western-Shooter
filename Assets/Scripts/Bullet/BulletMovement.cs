using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour
{
    
    public float selfDestructTime = 3.0f;
    float currentTime = 0;
    
    [HideInInspector]
    public float speed;
    
    void FixedUpdate()
    {
        rigidbody.velocity = transform.up * speed * Time.deltaTime;
        //transform.position += transform.up * speed * Time.deltaTime;
        if ((currentTime += Time.deltaTime) > selfDestructTime) {
            Destroy(gameObject);
        }
    }
}
