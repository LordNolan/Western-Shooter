using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour
{	
    public GameObject playerPrefab;
	
    // called in new game / death
    public void SpawnNewPlayer()
    {
        System.Diagnostics.Debug.Assert(GlobalParams.IsWorldGenComplete(), "[SpawnPlayer.SpawnNewPlayer] worldgen not complete and trying to instantiate new player.");
        
        // spawn player
        Vector2 pos = GetComponent<WorldGenerator>().getPlayerSpawnPosition();
        Instantiate(playerPrefab, new Vector3(pos.x, playerPrefab.transform.position.y, pos.y), playerPrefab.transform.rotation);
        GlobalParams.MarkPlayerSpawned();
    }
    
    // called when win a round
    public void ResetPlayerSpawn()
    {
        System.Diagnostics.Debug.Assert(GlobalParams.IsWorldGenComplete(), "[SpawnPlayer.ResetPlayerSpawn] worldgen not complete and trying to reset player spawn.");
        
        GameObject player = GameObject.FindWithTag("Player");
        // reset movement rotation first
        player.GetComponent<PlayerMovement>().Reset();
        // reset player's position
        Vector2 pos = GetComponent<WorldGenerator>().getPlayerSpawnPosition();
        player.transform.position = new Vector3(pos.x, playerPrefab.transform.position.y, pos.y);
        player.transform.rotation = playerPrefab.transform.rotation;
        GlobalParams.MarkPlayerSpawned();
      
    }
    
    public void DestroyPlayer()
    {
        // destroy player
        Destroy(GameObject.FindWithTag("Player"));
    }
}
