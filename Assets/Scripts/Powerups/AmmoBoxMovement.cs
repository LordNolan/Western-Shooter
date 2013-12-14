using UnityEngine;
using System.Collections;

public class AmmoBoxMovement : MonoBehaviour {
 
    float rotateSpeed = 100.0f;
    float tweenHeight = 0.2f;
    float tweenRiseTime = 0.4f;
    float tweenFallTime = 0.8f;
    float bounceDelay = 2.0f;
    float currentTime = 0;
	
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
        if ((currentTime += Time.deltaTime) >= bounceDelay)
        {
            currentTime = 0;
            Rise();
        }
        
        float yRotation = Time.deltaTime * rotateSpeed;
        yRotation = yRotation % 360;
        transform.Rotate(new Vector3(0, yRotation, 0));
    }
}
