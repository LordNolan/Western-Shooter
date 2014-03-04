using UnityEngine;
using System.Collections;

public class AmmoBoxMovement : MonoBehaviour
{
 
    public float rotateSpeed = 100.0f;
    public float tweenHeight = 0.3f;
    public float tweenRiseTime = 0.5f;
    public float tweenFallTime = 0.8f;
    public float bounceDelay = 2.0f;
    private float currentTime = 0;
	
    void Rise()
    {
        iTween.MoveBy(gameObject, iTween.Hash("y", tweenHeight, "easeType", "easeOutCubic", "time", tweenRiseTime, "onComplete", "Fall"));
    }
    
    void Fall()
    {
        iTween.MoveBy(gameObject, iTween.Hash("y", -tweenHeight, "easeType", "easeOutBounce", "time", tweenFallTime));
    }
    
    void Update()
    {
        if ((currentTime += Time.deltaTime) >= bounceDelay) {
            currentTime = 0;
            Rise();
        }
        
        float yRotation = Time.deltaTime * rotateSpeed;
        yRotation = yRotation % 360;
        transform.Rotate(new Vector3(0, yRotation, 0));
    }
}
