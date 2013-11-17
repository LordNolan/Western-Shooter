using UnityEngine;
using System.Collections;

public class SpriteFacePlayer : MonoBehaviour
{
    Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Vector3 camPos = cam.transform.position;
        transform.LookAt(new Vector3(camPos.x, transform.position.y, camPos.z));
    }
}
