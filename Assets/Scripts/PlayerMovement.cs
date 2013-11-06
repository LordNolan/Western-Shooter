using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public float movementSpeed = 10;
    public float turningSpeed = 60;
    void Update() {
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, 0);
        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, -vertical, 0);
    }
}
