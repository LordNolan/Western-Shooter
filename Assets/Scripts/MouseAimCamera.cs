using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour {
	
    public GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;
	
    void LateUpdate() {
		
		if (GlobalParams.IsWorldGenComplete())
		{
			float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
	        target.transform.Rotate(0, 0, horizontal);
	        float desiredAngle = target.transform.eulerAngles.y;
	        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
	        transform.position = target.transform.position - (rotation * offset);
	        transform.LookAt(target.transform);
			
			// added offset so player isn't middle of screen
			transform.position += new Vector3(0,.3f,0);
		}
    }
	
	void PlayerSpawned(GameObject player)
	{
		target = player;
		offset = target.transform.position - transform.position;
	}
}
