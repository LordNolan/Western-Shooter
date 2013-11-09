using UnityEngine;
using System.Collections;

public class GlobalParams {
	
	static bool worldGenComplete = false;
	
	void Start()
	{
		Physics.IgnoreLayerCollision(8,8);
	}
	public static void MarkWorldGenComplete()
	{
		worldGenComplete = true;
	}
	
	public static bool IsWorldGenComplete()
	{
		return worldGenComplete;
	}
}