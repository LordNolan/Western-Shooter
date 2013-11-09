using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	private float movementSpeed = 8;
	
    void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);
        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, -vertical, 0);
		
		rigidbody.velocity = Vector3.zero;
    }
}
