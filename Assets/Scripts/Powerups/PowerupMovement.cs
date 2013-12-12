using UnityEngine;
using System.Collections;

public class PowerupMovement : MonoBehaviour {
 
    float rotateSpeed = 100.0f;
    float tweenDelay = 0.2f;
    float tweenHeight = 0.9f;
    float tweenTime = 0.5f;
    float aliveTime = 3.0f;
    float currentTime = 0;
    
    float yRotation;
    
    void Start() 
    {
        iTween.MoveBy(gameObject, iTween.Hash("y", tweenHeight, "easeType", "easeOutElastic", "delay", tweenDelay, "time", tweenTime));
    }
    
	void Update () 
    {
        if ((currentTime += Time.deltaTime) > aliveTime) 
            Destroy(gameObject);
        
        yRotation = Time.deltaTime * rotateSpeed;
        yRotation = yRotation % 360;
        transform.Rotate(new Vector3(0, yRotation, 0));
	}
}