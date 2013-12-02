using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("y", 1, "easeType", "easeOutElastic", "loopType", "pingpong", "delay", .2, "time", 1.0f));
	}
}

