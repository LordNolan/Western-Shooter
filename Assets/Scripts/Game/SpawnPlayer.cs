using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour
{	
    public GameObject playerPrefab;
	
    // called in new game / death
    public void SpawnNewPlayer()
    {
        if (GlobalParams.IsWorldGenComplete()) {
            // spawn player
            Vector2 pos = GetComponent<WorldGenerator>().getPlayerSpawnPosition();
            Instantiate(playerPrefab, new Vector3(pos.x, playerPrefab.transform.position.y, pos.y), playerPrefab.transform.rotation);
            GlobalParams.MarkPlayerSpawned();
        } else {
            Debug.LogError("[SpawnPlayer.SpawnNewPlayer] worldgen not complete and trying to instantiate new player.");
        }
    }
    
    // called when win a round
    public void ResetPlayerSpawn()
    {
        if (GlobalParams.IsWorldGenComplete()) {
            Vector2 pos = GetComponent<WorldGenerator>().getPlayerSpawnPosition();
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(pos.x, playerPrefab.transform.position.y, pos.y);
            GlobalParams.MarkPlayerSpawned();
        } else {
            Debug.LogError("[SpawnPlayer.ResetPlayerSpawn] worldgen not complete and trying to reset player spawn.");
        }
    }
    
    public void DestroyPlayer()
    {
        // destroy player
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
}
