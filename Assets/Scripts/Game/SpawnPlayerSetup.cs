using UnityEngine;
using System.Collections;

public class SpawnPlayerSetup : MonoBehaviour
{	
    public GameObject playerPrefab;
	 
    private bool playerSpawned = false;
	
    void Update()
    {
        if (!playerSpawned && GlobalParams.IsWorldGenComplete()) {
            // spawn player
            Vector2 pos = GetComponent<WorldGenerator>().getPlayerSpawnPosition();
            GameObject player = (GameObject) Instantiate(playerPrefab, new Vector3(pos.x, playerPrefab.transform.position.y, pos.y), playerPrefab.transform.rotation);
			
            // tell camera to link up to player
            GameObject.Find("Camera").BroadcastMessage("PlayerSpawned", player);
            playerSpawned = true;
        }
    }
	
    public bool DidPlayerSpawn()
    {
        return playerSpawned;
    }
    
    public void DestroyPlayer()
    {
        // destroy player
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        
        // reset spawn bool
        playerSpawned = false;
    }
}
