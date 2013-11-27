using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 8;
	
    void FixedUpdate()
    {
        if (!GlobalParams.InNonPlayingState()) {
            float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            transform.Translate(horizontal, 0, 0);
            float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            transform.Translate(0, 0, vertical);
		
            rigidbody.velocity = Vector3.zero;
        }
    }
}
