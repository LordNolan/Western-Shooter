using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject enemy1;
	
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
	
    void GenerateWorld()
    {
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
        //KeepSingleChest(); // only keep one chest
        SpawnEnemies();
        
        // tell game we're done with world gen.
        GlobalParams.MarkWorldGenComplete();
    }
	
    void SpawnEnemies()
    {
        foreach (Tile t in floorTileList) {
            if (FarFromPlayerSpawn(t.getPosition())) {
                if (Random.Range(0, 10) <= 0) {
                    InstantiateEnemy(enemy1, t.getPosition());
                }
            }
        }
    }
    
    void InstantiateEnemy(GameObject enemy, Vector2 position)
    {
        Instantiate(enemy, new Vector3(position.x * tileSizeOffset, enemy.transform.position.y, position.y * tileSizeOffset), Quaternion.identity);
    }
    
    // player always spawns at 0,0
    bool FarFromPlayerSpawn(Vector2 pos)
    {
        return pos.x + pos.y > 6;
    }
    
    void GenerateSpawnpoint()
    {
        Tile t = new Tile(Vector2.zero);
        floorTileList.Add(t);
        playerSpawn = t;
    }
	
    public Vector2 getPlayerSpawnPosition()
    {
        return playerSpawn.getPosition();
    }
	
    void GenerateWalls()
    {
        // for each floor piece put up walls if there's no adjacent floor piece and no existing wall
        foreach (Tile t in floorTileList) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    Vector2 offset = new Vector2(x, y);
                    if (!TileExistsWithPosition(t.getPosition() + offset, floorTileList) && !TileExistsWithPosition(t.getPosition() + offset, wallTileList)) {
                        InstantiateTile(wallTile, t.getPosition() + offset);
                        wallTileList.Add(new Tile(t.getPosition() + offset));
                    }
                }
            }
        }
    }
	
    // remove all chests but furthest
    void KeepSingleChest()
    {
        // find furthest chest from origin
        Tile furthestTile = treasureList [0];
        float furthestDist = Vector2.Distance(Vector2.zero, furthestTile.getPosition());
        foreach (Tile t in treasureList) {
            float dist = Vector2.Distance(Vector2.zero, t.getPosition());
            if (furthestDist < dist) {
                furthestTile = t;
                furthestDist = dist;
            }
        }
		
        // set treasure spawn and draw tile
        treasureSpawn = furthestTile;
        InstantiateTile(treasureTile, treasureSpawn.getPosition());
		
        // clear treasure list
        treasureList.Clear();
    }
	
    void Update()
    {
        // regenerate on keypress
        if (!generating && Input.GetKeyDown(KeyCode.R)) {
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
	
    void InstantiateTile(GameObject tile, Vector2 position)
    {
        // instantiate tile prefab in unity and set it as child object to World
        GameObject newTile = (GameObject) Instantiate(tile, new Vector3(position.x * tileSizeOffset, 0, position.y * tileSizeOffset), Quaternion.identity);
        newTile.transform.parent = gameObject.transform;
    }
	
    bool TileExistsWithPosition(Vector2 position, List<Tile> tileGroup)
    {
        var matches = tileGroup.Where(x => x.getPosition() == position);
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
