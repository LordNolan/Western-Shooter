using UnityEngine;
using System.Collections;

public class BlinkStart : MonoBehaviour
{
    float currentTime;
    float currentRate;
    bool selected;
    bool loading;
    public float blinkRate = 0.2f;
    public float selectRate = 0.1f;
    
    void Update()
    {
        if (loading) {
            Application.LoadLevel("world");
        }
        if (selected && !audio.isPlaying) {
            GameObject.Find("LoadingBackground").guiTexture.enabled = true;
            GameObject.Find("LoadingBackground").transform.GetChild(0).guiTexture.enabled = true;
            loading = true;
        }
        
        if (!selected && Input.GetButtonDown("Fire3")) {
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
