using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
 
    public float speed;
    bool moveRight = true;
    
    // adding delay so that if we hit seam between two walls, we don't flip flag twice and go through wall
    public float collisionDelay = .5f;
    float currentTime = 0;
    
	void Update () 
    {
        if (GlobalParams.IsMobAIDelayComplete() && !GlobalParams.InNonPlayingState() && !GetComponent<Hitpoints>().mobDead) {
            transform.Translate(Vector3.right * speed * GetDirection() * Time.deltaTime);
        }
        
        currentTime += Time.deltaTime;
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
        if (currentTime >= collisionDelay)
        {
            currentTime = 0;
            return true;
        }
        else 
            return false;
    }
}
