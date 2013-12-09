using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 8;
    public float rotateSpeed = 3;
    public bool invertedAxis = false;
    float rotationX = 0;
    float rotationY = 0;
    Vector3 forwardVector;
    
    public float pu_SpeedBoost = 0f;
    
    void Start()
    {
        forwardVector = Vector3.zero;
    }
    
    void FixedUpdate()
    {
        if (!GlobalParams.InNonPlayingState()) {
            
            forwardVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // get input
            // normalize vector so we have unit vector in direction of input
            if (forwardVector.magnitude > 1) 
                forwardVector.Normalize(); 
            forwardVector *= (movementSpeed + pu_SpeedBoost) * Time.deltaTime; // set magnitude 
            
            // ensures that we're always moving on x/z plane no matter the x rotation (looking up/down)
            forwardVector = Quaternion.Euler(-transform.eulerAngles.x, 0, 0) * forwardVector;
            CharacterController controller = GetComponent<CharacterController>();
            controller.SimpleMove(transform.TransformDirection(forwardVector));
            
            rotationX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            rotationX = rotationX % 360; // we just want remainder so we don't have crazy rotation values
            rotationY += Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime * isInvertedAxis();
            rotationY = Mathf.Clamp(rotationY, -80.0f, 80.0f); // don't rotate further than 80 so we don't flip
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        }
    }
    
    int isInvertedAxis()
    {
        return (invertedAxis) ? 1 : -1;
    }
    
    public bool IsMoving()
    {
        return forwardVector.magnitude > 0;
    }
}
