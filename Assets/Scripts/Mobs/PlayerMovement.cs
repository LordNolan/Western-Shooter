using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 8;
    public float rotateSpeed = 3;
    public bool invertedAxis = false;
    float rotationX = 0;
    float rotationY = 0;
    
    void Update()
    {
        if (!GlobalParams.InNonPlayingState()) {
            float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            
            // ensures that we're always moving on x/z plane no matter the x rotation (looking up/down)
            Vector3 forwardVector = new Vector3(horizontal, 0, vertical);
            forwardVector = Quaternion.Euler(-transform.eulerAngles.x, 0, 0) * forwardVector;
            transform.Translate(forwardVector);
            
            rotationX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            rotationX = rotationX % 360; // we just want remainder so we don't have crazy rotation values
            rotationY += Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime * isInvertedAxis();
            rotationY = Mathf.Clamp(rotationY, -80.0f, 80.0f); // don't rotate further than 80 so we don't flip
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);

            rigidbody.velocity = Vector3.zero;
        }
    }
    
    int isInvertedAxis()
    {
        return (invertedAxis) ? 1 : -1;
    }
}
