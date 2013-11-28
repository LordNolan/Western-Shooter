using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 8;
    public float rotateSpeed = 5;
    
    void FixedUpdate()
    {
        if (!GlobalParams.InNonPlayingState()) {
            float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            transform.Translate(horizontal, 0, 0);
            float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            transform.Translate(0, 0, vertical);
            float rotation = Input.GetAxis("Mouse X") * rotateSpeed;
            transform.Rotate(0, rotation, 0);
            
            rigidbody.velocity = Vector3.zero;
        }
    }
}
