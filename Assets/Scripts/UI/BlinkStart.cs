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
        if (selected && !audio.isPlaying) {
            Application.LoadLevel("world");
        }
        
        if (!selected && Input.GetKey(KeyCode.Space)) {
            selected = true;
            audio.Play();
        }
        
        currentRate = (selected) ? selectRate : blinkRate;
        if ((currentTime += Time.deltaTime) > currentRate) {
            currentTime = 0;
            guiTexture.enabled = !guiTexture.enabled;
        }
    }
}
