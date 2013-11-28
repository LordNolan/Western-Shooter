using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour
{
    public float rotateSpeed = 5;
    public bool invertedAxis = false;
    void LateUpdate()
    {
        if (GlobalParams.IsWorldGenComplete()) {
            if (!GlobalParams.InNonPlayingState()) {
                float vertical = Input.GetAxis("Mouse Y") * rotateSpeed * isInvertedAxis();
                transform.Rotate(vertical, 0, 0);
            }
        }
    }
    
    int isInvertedAxis()
    {
        return (invertedAxis) ? 1 : -1;
    }
}