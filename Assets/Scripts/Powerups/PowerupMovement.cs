﻿using UnityEngine;
using System.Collections;

public class PowerupMovement : MonoBehaviour {
 
    float rotateSpeed = 100.0f;
    float tweenHeight = 0.45f;
    float tweenTime = 0.4f;
    float aliveTime = 3.0f;
    float currentTime = 0;
    
    void Start() 
    {
        iTween.MoveBy(gameObject, iTween.Hash("y", tweenHeight, "easeType", "easeOutBack", "time", tweenTime));
    }
    
	void Update () 
    {
        if ((currentTime += Time.deltaTime) > aliveTime) 
            Destroy(gameObject);
        
        float yRotation = Time.deltaTime * rotateSpeed;
        yRotation = yRotation % 360;
        transform.Rotate(new Vector3(0, yRotation, 0));
	}
}