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
            Instantiate(playerPrefab, new Vector3(pos.x, playerPrefab.transform.position.y, pos.y), playerPrefab.transform.rotation);
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
