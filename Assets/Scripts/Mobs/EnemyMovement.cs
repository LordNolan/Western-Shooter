using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
 
    public float speed;
    bool moveRight = true;
    
    // adding delay so that if we hit seam between two walls, we don't flip flag twice and go through wall
    public float collisionDelay = .01f;
    float currentTime;
    
    public bool canMove = false;
    
    void Start()
    {
        // enable mob to hit wall from start
        currentTime = collisionDelay;
    }
    void Update()
    {
        if (canMove && !GetComponent<Hitpoints>().mobDead) {
            transform.Translate(Vector3.right * speed * GetDirection() * Time.deltaTime);
            currentTime += Time.deltaTime;
        }
    }
    
    float GetDirection()
    {
        return (moveRight) ? 1.0f : -1.0f;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // mob hits wall
        if (collision.gameObject.CompareTag("Wall") && CanCollideWithWall()) {
            moveRight = !moveRight;
        }
    }
    
    // checks if enough time has passed between collisions
    bool CanCollideWithWall()
    {
        if (currentTime >= collisionDelay) {
            currentTime = 0;
            return true;
        } else 
            return false;
    }
}
