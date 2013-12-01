using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject enemy1;
    public GameObject scenery1;
    public GameObject scenery2;
	
    private List<Tile> floorTileList;
    private List<Tile> wallTileList;
    private List<Tile> treasureList;
    private List<FloorWalker> walkerList;
    private List<FloorWalker> deadWalkerList;
    private List<FloorWalker> childWalkerList;
	
    private bool generating = false;
    private int walkerSteps = 150;
    private Tile treasureSpawn;
    private Tile playerSpawn;
	
    private float tileSizeOffset = 1f;
	
    private enum TileType
    {
        Treasure,
        Wall
    }
	
    void Start()
    {	
        floorTileList = new List<Tile>();
        wallTileList = new List<Tile>();
        treasureList = new List<Tile>();
        walkerList = new List<FloorWalker>();
        deadWalkerList = new List<FloorWalker>();
        childWalkerList = new List<FloorWalker>();
        walkerList.Add(new FloorWalker(Vector2.zero, walkerSteps));
		
        GenerateWorld();
    }
	
    public void GenerateWorld()
    {
        // remove all child objects from world
        foreach (Transform child in gameObject.transform) {
            Destroy(child.gameObject);
        }
        
        // reset tile Lists
        floorTileList.Clear();
        wallTileList.Clear();
        
        // reset walkerList
        walkerList.Clear();
        walkerList.Add(new FloorWalker(Vector2.zero, walkerSteps));
        generating = true;
		
        GenerateSpawnpoint(); // do spawnpoint
		
        // generate world
        while (generating) {
            foreach (FloorWalker walker in walkerList) {
                if (walker.HasMovesLeft()) {
                    int actionNum = walker.Move();
					
                    // 50% chance of 2x2 room
                    if (Random.Range(0, 2) == 0) {
                        CreateTwoByTwoRoom(walker.getPosition(), actionNum);
                    } else {
                        AddTile(walker.getPosition(), actionNum);
                    }
					
                    // small chance of spawning another walker
                    TrySpawnAnotherWalker(walker.getPosition(), walker.getMovesLeft());
                } else {
                    deadWalkerList.Add(walker);
                }
            }
			
            // remove dead walkers from walkerList
            foreach (FloorWalker deadWalker in deadWalkerList) {
                walkerList.Remove(deadWalker);
            }
            deadWalkerList.Clear();
			
            // add in any child walkers
            walkerList.AddRange(childWalkerList);
            childWalkerList.Clear();
			
            if (walkerList.Count() == 0)
                generating = false;
        }
		
        GenerateWalls();   // create all the walls
        GenerateChest();   // only keep one chest
        SpawnEnemies();
        
        // tell game we're done with world gen.
        GlobalParams.MarkWorldGenComplete();
    }
	
    void SpawnEnemies()
    {
        int mobcount = 0;
        foreach (Tile t in floorTileList) {
            if (t.GetPosition() != treasureSpawn.GetPosition() && FarFromPlayerSpawn(t.GetPosition())) {
                if (Random.Range(0, 10) <= 0) {
                    if (Random.Range(0, 4) == 1) {
                        InstantiateObject(enemy1, t.GetPosition());
                        mobcount++;
                    } else {
                        if (Random.Range(0, 2) == 1)
                            InstantiateScenery(scenery1, t.GetPosition());
                        else
                            InstantiateScenery(scenery2, t.GetPosition());
                    }
                }
            }
        }
        GameObject.Find("Environment").GetComponent<GameController>().SetMobCount(mobcount);
    }
    
    void GenerateSpawnpoint()
    {
        Tile t = new Tile(Vector2.zero);
        floorTileList.Add(t);
        playerSpawn = t;
    }
	
    public Vector2 getPlayerSpawnPosition()
    {
        return playerSpawn.GetPosition();
    }
	
    void GenerateWalls()
    {
        // for each floor piece put up walls if there's no adjacent floor piece and no existing wall
        foreach (Tile t in floorTileList) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    Vector2 offset = new Vector2(x, y);
                    if (!TileExistsWithPosition(t.GetPosition() + offset, floorTileList) && !TileExistsWithPosition(t.GetPosition() + offset, wallTileList)) {
                        InstantiateTile(wallTile, t.GetPosition() + offset);
                        wallTileList.Add(new Tile(t.GetPosition() + offset));
                    }
                }
            }
        }
    }
	
    // remove all chests but furthest
    void GenerateChest()
    {
        // find furthest chest from origin
        Tile furthestTile = treasureList[0];
        float furthestDist = Vector2.Distance(Vector2.zero, furthestTile.GetPosition());
        foreach (Tile t in treasureList) {
            float dist = Vector2.Distance(Vector2.zero, t.GetPosition());
            if (furthestDist < dist) {
                furthestTile = t;
                furthestDist = dist;
            }
        }
		
        // set treasure spawn
        treasureSpawn = furthestTile;
        
        // get proper rotation so that chest always faces open tile
        Quaternion rotation = GetChestRotationFacingOpenTile(treasureSpawn.GetPosition());
        
        // create the chest
        InstantiateTile(treasureTile, new Vector3(treasureSpawn.GetPosition().x, treasureTile.transform.position.y, treasureSpawn.GetPosition().y), rotation);
		
        // clear treasure list
        treasureList.Clear();
    }
    
    Quaternion GetChestRotationFacingOpenTile(Vector2 chestPosition)
    {
        if (TileExistsWithPosition(new Vector2(chestPosition.x - 1, chestPosition.y), floorTileList)) { // left 
            return Quaternion.Euler(new Vector3(0, 90, 0));
        } else if (TileExistsWithPosition(new Vector2(chestPosition.x, chestPosition.y + 1), floorTileList)) { // up
            return Quaternion.Euler(new Vector3(0, 180, 0));
        } else if (TileExistsWithPosition(new Vector2(chestPosition.x + 1, chestPosition.y), floorTileList)) { // right
            return Quaternion.Euler(new Vector3(0, 270, 0));
        } else if (TileExistsWithPosition(new Vector2(chestPosition.x, chestPosition.y - 1), floorTileList)) { // down (default)
            return Quaternion.identity;
        } else {
            return Quaternion.identity;
        }
    }
	
    void Update()
    {
        // regenerate on keypress
        if (!generating && Input.GetKeyDown(KeyCode.R)) {
            GenerateWorld();
        }
    }
	
    void AddTile(Vector2 position, int actionNum)
    {
        if (!TileExistsWithPosition(position, floorTileList)) {
            // create tile object for adding to list
            Tile t = new Tile(position);
            floorTileList.Add(t);
			
            // based on walker action, create different tile
            // (0:none, 1:left, 2:right:, 3:around)
            switch (actionNum) {
                case 3:
                    treasureList.Add(t);
                    break;
                default:
                    break;
            }
        }
    }
    
    void InstantiateObject(GameObject obj, Vector2 position)
    {
        GameObject newObj = (GameObject)Instantiate(obj, new Vector3(position.x * tileSizeOffset, obj.transform.position.y, position.y * tileSizeOffset), obj.transform.rotation);
        newObj.transform.parent = gameObject.transform;
    }
    
    void InstantiateScenery(GameObject obj, Vector2 position)
    {
        Quaternion randomRotation = Quaternion.Euler(new Vector3(270, Random.Range(0, 4) * 90, 0));
        GameObject newObj = (GameObject)Instantiate(obj, new Vector3(position.x * tileSizeOffset, obj.transform.position.y, position.y * tileSizeOffset), randomRotation);
        newObj.transform.parent = gameObject.transform;
    }
	
    void InstantiateTile(GameObject tile, Vector2 position)
    {
        InstantiateTile(tile, new Vector3(position.x, 0, position.y));
    }
    
    void InstantiateTile(GameObject tile, Vector3 position)
    {
        // instantiate tile prefab in unity and set it as child object to World
        InstantiateTile(tile, new Vector3(position.x * tileSizeOffset, position.y, position.z * tileSizeOffset), Quaternion.identity);
    }
    
    void InstantiateTile(GameObject tile, Vector3 position, Quaternion rotation)
    {
        // instantiate tile prefab in unity and set it as child object to World
        GameObject newTile = (GameObject)Instantiate(tile, new Vector3(position.x * tileSizeOffset, position.y, position.z * tileSizeOffset), rotation);
        newTile.transform.parent = gameObject.transform;
    }
	
    bool TileExistsWithPosition(Vector2 position, List<Tile> tileGroup)
    {
        var matches = tileGroup.Where(x => x.GetPosition() == position);
        return (matches.Count() > 0);
    }

    void CreateTwoByTwoRoom(Vector2 position, int action)
    {
        // x x
        // p x
        // only apply action to position of walker
        AddTile(position, action);
        AddTile(position + new Vector2(1, 0), 0);
        AddTile(position + new Vector2(1, 1), 0);
        AddTile(position + new Vector2(0, 1), 0);
    }
	
    // player always spawns at 0,0
    bool FarFromPlayerSpawn(Vector2 pos)
    {
        return Vector2.Distance(pos, Vector2.zero) > 5;
    }
    
    void TrySpawnAnotherWalker(Vector2 position, int parentMovesLeft)
    {
        // depends on how many walkers exist
        // should have less moves so we aren't walking forever
		
        // 10% chance to spawn walker
        if (Random.Range(0, 10) == 0) {
            childWalkerList.Add(new FloorWalker(position, Mathf.CeilToInt(parentMovesLeft / 4)));
        }
    }
}
