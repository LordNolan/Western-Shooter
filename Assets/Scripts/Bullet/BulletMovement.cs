using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
	
	private float speed = 40f;
	
	void Start () {
	
	}
	
	void FixedUpdate () {
		transform.position += transform.up * speed * Time.deltaTime;
	}
}
