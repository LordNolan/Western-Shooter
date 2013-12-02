using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
 
    public float speed;
    bool moveRight = true;
    
	void Update () 
    {
        if (GlobalParams.IsMobAIDelayComplete() && !GlobalParams.InNonPlayingState() && !GetComponent<Hitpoints>().mobDead) {
            transform.Translate(Vector3.right * speed * GetDirection() * Time.deltaTime);
        }
	}
    
    float GetDirection()
    {
        return (moveRight) ? 1.0f : -1.0f;
    }
    
    void OnCollisionEnter(Collision collision) 
    {
        // mob hits wall
        if (collision.gameObject.CompareTag("Wall")) {
            moveRight = !moveRight;
        }
    }
}
