using UnityEngine;
using System.Collections;

public class GlobalParams {
	
	static bool worldGenComplete = false;
	
	public static void MarkWorldGenComplete()
	{
		worldGenComplete = true;
	}
	
	public static bool IsWorldGenComplete()
	{
		return worldGenComplete;
	}
}