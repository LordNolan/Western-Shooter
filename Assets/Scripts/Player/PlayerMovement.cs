using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private float originalMovementSpeed;
    public float rotateSpeed = 3;
    public bool invertedAxis = false;
    float rotationX = 180;
    float rotationY = 0;
    Vector3 forwardVector;
    float movementMagnitude;
    public float playerRecoilDistance = 15.0f;
    public float pu_SpeedBoost = 0f;
    
    [HideInInspector]
    public bool
        shouldRecoil = false;
    
    void Start()
    {
        forwardVector = Vector3.zero;
        originalMovementSpeed = movementSpeed;
    }
    
    void FixedUpdate()
    {
        if (!GlobalParams.InNonPlayingState()) {
            forwardVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // get input
            // normalize vector so we have unit vector in direction of input
            if (forwardVector.magnitude > 1) 
                forwardVector.Normalize();
            movementMagnitude = forwardVector.magnitude;
            forwardVector *= (movementSpeed + pu_SpeedBoost) * Time.deltaTime; // set magnitude 
            
            // ensures that we're always moving on x/z plane no matter the x rotation (looking up/down)
            forwardVector = Quaternion.Euler(-transform.eulerAngles.x, 0, 0) * forwardVector;
            CharacterController controller = GetComponent<CharacterController>();
            controller.SimpleMove(transform.TransformDirection(forwardVector));
            
            rotationX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            rotationX = rotationX % 360;
            rotationY += Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime * isInvertedAxis();
            if (shouldRecoil) { // we fired pistol, jerk camera
                shouldRecoil = false;
                iTween.RotateAdd(gameObject, iTween.Hash("x", playerRecoilDistance, "easeType", "easeOutCubic", "time", 0.15f));
            }
            rotationY = Mathf.Clamp(rotationY, -80.0f, 80.0f); // don't rotate further than 80 so we don't flip
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        }
    }
    
    // resets rotationX so that we're always facing correct way in new level
    public void Reset()
    {
        rotationX = 180;
    }
    
    int isInvertedAxis()
    {
        return (invertedAxis) ? 1 : -1;
    }
    
    public bool IsMoving()
    {
        return movementMagnitude > 0;
    }
    
    public float GetForwardMagnitude()
    {
        return movementMagnitude;
    }
    
    public void ResetSpeed()
    {
        movementSpeed = originalMovementSpeed;
    }
}
