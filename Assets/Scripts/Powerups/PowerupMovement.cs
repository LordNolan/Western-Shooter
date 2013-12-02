using UnityEngine;
using System.Collections;

public class PowerupMovement : MonoBehaviour {
 
    public float rotateSpeed;
    public float tweenDelay;
    public float tweenHeight;
    public float tweenTime;
    public float aliveTime;
    float currentTime = 0;
    
    float yRotation;
    
    void Start() {
         iTween.MoveBy(gameObject, iTween.Hash("y", tweenHeight, "easeType", "easeOutElastic", "delay", tweenDelay, "time", tweenTime));
    }
	void Update () {
        if ((currentTime += Time.deltaTime) > aliveTime) 
            Destroy(gameObject);
        
        yRotation = Time.deltaTime * rotateSpeed;
        yRotation = yRotation % 360;
        transform.Rotate(new Vector3(0, yRotation, 0));
	}
}