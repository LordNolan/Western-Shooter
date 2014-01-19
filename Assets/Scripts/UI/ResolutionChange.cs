using UnityEngine;
using System.Collections;

public class ResolutionChange : MonoBehaviour
{
    private int sizeX;
    private int sizeY;
    private float aspectRatio;
    private bool updated;
    
    void Start()
    {
        if (guiTexture != null) {
            sizeX = guiTexture.texture.width;
            sizeY = guiTexture.texture.height;
        }
    }
    
    void Update()
    {
        if (!updated && GlobalParams.IsPlayerSpawned()) {
            ChangeResolution();
            updated = true;
        }
    }
    
    public void UpdateResolutionAgain()
    {
        updated = false;
    }
    
    void ChangeResolution()
    {
        float ratio = Camera.main.aspect;
        if (guiTexture != null) {
            var v = sizeX / (sizeY * ratio);
            transform.localScale = new Vector3(v * transform.localScale.y, transform.localScale.y, 1);
        } else {
            transform.localScale = new Vector3(1.7777f / ratio, ratio, 1);
        }
    }
}
