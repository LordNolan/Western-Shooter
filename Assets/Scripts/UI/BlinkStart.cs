using UnityEngine;
using System.Collections;

public class BlinkStart : MonoBehaviour
{
    float currentTime;
    float currentRate;
    bool selected;
    public float blinkRate = 0.2f;
    public float selectRate = 0.1f;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            selected = true;
        }
        
        currentRate = (selected) ? selectRate : blinkRate;
        if ((currentTime += Time.deltaTime) > currentRate) {
            currentTime = 0;
            guiTexture.enabled = !guiTexture.enabled;
        }
    }
}
