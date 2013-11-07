using UnityEngine;
using System.Collections;

public class SpawnPlayerSetup : MonoBehaviour
{	
	public GameObject playerPrefab;
	 
	private bool playerSpawned = false;
	
	void Update () 
	{
		if (!playerSpawned && GlobalParams.IsWorldGenComplete())
		{
			// spawn player
			Vector2 pos = GetComponent<WorldGenerator>().getPlayerSpawnPosition();
			GameObject player = (GameObject)Instantiate(playerPrefab, pos, playerPrefab.transform.rotation);
			
			// tell camera to link up to player
			GameObject.Find("Camera").BroadcastMessage("PlayerSpawned", player);
			playerSpawned = true;
		}
	}
	
	public void ResetPlayerSpawn()
	{
		// destroy player
		// reset spawn bool
	}
}
