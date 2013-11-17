using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour
{
    public float speed;
	
    void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
