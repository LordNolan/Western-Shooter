using UnityEngine;
using System.Collections;

// TODO: will need to show back of enemy if between player and camera
public class EnemyFaceCamera : MonoBehaviour
{
    private Camera cam;
    
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
