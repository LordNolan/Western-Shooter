using UnityEngine;
using System.Collections;

public class PixelizeOnHit : MonoBehaviour
{
    public int pixelizeAmount = 4;
    public float pixelizeTime = 0.5f;
    private SimplePixelizer cameraPixel;
	
    void Start()
    {
        cameraPixel = Camera.main.GetComponent<SimplePixelizer>();    
    }
	
    public void Hit()
    {
        cameraPixel.pixelize = pixelizeAmount;
        iTween.ValueTo(gameObject, iTween.Hash("from", pixelizeAmount, "to", 1.0, "time", pixelizeTime, "onupdate", "Tweened"));
    }
    
    public void Tweened(double val)
    {
        cameraPixel.pixelize = (int)val;
    }
}