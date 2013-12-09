using UnityEngine;
using System.Collections;

// gun sway based on lissajou curve
// formula found in example 3 of http://www.intmath.com/trigonometric-graphs/7-lissajous-figures.php
public class Lissajou : MonoBehaviour 
{
    float local_X;
    float local_Y;
    float t = 0;
    
    public float xAmplitude;
    public float yAmplitude;
    public float speed;
    
    void Start()
    {
        local_X = transform.localPosition.x;
        local_Y = transform.localPosition.y;
    }
    
	void Update() 
    {
        // only when we are moving should we sway the pistol
        if (transform.parent.GetComponent<PlayerMovement>().IsMoving()) 
            t += Time.deltaTime * speed;
        float x = Mathf.Cos(t + Mathf.PI/4) * xAmplitude;
        float y = Mathf.Sin(t * 2) * yAmplitude; // invert so we get parabola going down
        transform.localPosition = new Vector3(local_X + x, local_Y + y, transform.localPosition.z);
	}
}