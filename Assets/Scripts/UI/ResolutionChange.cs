using UnityEngine;
using System.Collections;

public class ResolutionChange : MonoBehaviour
{
    private int sizeX;
    private int sizeY;
    private float aspectRatio;
    private bool updated;
    
    public bool letterbox; // for title screen resolutions
    public bool nonGameElement; // for non HUD elements (loading screen)
    
    void Start()
    {
        if (guiTexture != null) {
            sizeX = guiTexture.texture.width;
            sizeY = guiTexture.texture.height;
        }
    }
    
    void Update()
    {
        if (!updated && letterbox) {
            Letterbox();
            updated = true;
        }
        
        if (!updated && nonGameElement) {
            ChangeResolution();
            updated = true;
        }
        
        if (!updated && GlobalParams.IsPlayerSpawned()) {
            ChangeResolution();
            updated = true;
        }
    }
    
    public void UpdateResolutionAgain()
    {
        updated = false;
    }
    
    void Letterbox()
    {
        float ratio = Camera.main.aspect;
        if (guiTexture != null) {
            var v = sizeX / (sizeY * ratio);
            if ((transform.localScale.y / v) <= 1) {
                transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y / v, 1); // top bars
            } else {
                transform.localScale = new Vector3(v * transform.localScale.y, transform.localScale.y, 1); // side bars
            }
            
        }
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
