using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject enemy1; // cactus bandit
    public GameObject enemy2; // bug bandit
    public GameObject scenery1; // barrel
    public GameObject scenery2; // cactus
	
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
        
        // reset world array
        m = new int[H,W];
        
        // reset tile Lists
        floorTileList.Clear();
        wallTileList.Clear();
        
        // reset walkerList
        walkerList.Clear();
        walkerList.Add(new FloorWalker(Vector2.zero, walkerSteps));
        generating = true;
		
        GenerateCanyons();     // generate world IJ style
        GenerateSpawnArea();
        GenerateCanyonWalls();
        
        // GenerateChest();    // only keep one chest
        SpawnEnemies();
        
        // tell game we're done with world gen.
        GlobalParams.MarkWorldGenComplete();
    }
    
    #region IJ CANYON MAP GENERATOR
    static int C = 8;
    static int W = 60;
    static int H = 60;
    int[,] m = new int[H,W];
    
    void GenerateCanyons()
    {
        for(int z = 0; z < 4; z++)
        {
            Zigzag();
            Smooth();
        }
        
        for(int x = 0; x < W; x++) {
            for(int y = 0; y < H; y++) {
                if (m[x,y] == 3) {
                    AddTile(new Vector2(x,y),0);
                }
            }
        }
    }
    
    void GenerateCanyonWalls()
    {
        for(int x = 0; x < W; x++) {
            for(int y = 0; y < H; y++) {
                if (m[x,y] == 2) {
                    InstantiateTile(wallTile, new Vector2(x,y));
                } else if (m[x,y] == 1) {
                    InstantiateTile(wallTile, new Vector2(x,y));
                    InstantiateTile(wallTile, new Vector3(x,1,y));
                } else if (m[x,y] == 0) {
                    InstantiateTile(wallTile, new Vector3(x,2,y));
                }
            }
        }
    }
    
    void Zigzag()
    {
        int x = C;
        int y = C;
        while (x < W-C-1 || y < H-C-1) 
        {
            int dx = 0;
            int dy = 0;
            if      (x == W-C-1)            { dy = Random.Range(1, Mathf.Min(H-C-1-y, 3)); }
            else if (y == H-C-1)            { dx = Random.Range(1, Mathf.Min(W-C-1-x, 3)); }
            else if (Random.Range(0,2) < 1) { dy = Random.Range(1, Mathf.Min(H-C-1-y, 3)); }
            else                            { dx = Random.Range(1, Mathf.Min(W-C-1-x, 3)); }
            Carve(x, y, dx, dy);
            x += dx;
            y += dy;
        }
        m[W-C-1,H-C-1] = 3;
    }
    
    void Carve(int x, int y, int dx, int dy) 
    {
        while(dx > 0 || dy > 0) {
            m[x,y] = 3;
            if (Random.Range(0,10) < 2) {
                for (int i=-1; i<=1; i++) {
                    for (int j=-1; j<=1; j++) {
                        m[x+i,y+j] = 3;
                    }
                }
            }
            if (dx > 0) { dx--; x++; }
            if (dy > 0) { dy--; y++; }
        }
    }
    
    void Smooth() {
        for(int x = 0; x < W; x++) {
            for(int y = 0; y < H; y++) {
                if      (m[x,y] == 3)        { continue; }
                if      (InRange(x, y, 3, 3)) { m[x,y] = Random.Range(0,100) < 75 ? 2 : 1; }
                else if (InRange(x, y, 3, 4)) { m[x,y] = 1; }
            }
        }
    }

    bool InRange(int x, int y, int val, int range) {
        for(int a = -range; a <= range; a++) {
            for(int b = -range; b <= range; b++) {
                if (Get(x+a, y+b) == val) { return true; }
            }
        }
        return false;
    }

    int Get(int x, int y) 
    {
        if (x < 0 || y < 0 || x >= W || y >= H) { return 0; }
        return m[x,y];
    }
    
    
    #endregion
    
    void GenerateFloor()
    {
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
    }
    
    void GenerateSpawnArea()
    {
        var maxY = floorTileList.Max(tile => tile.GetPosition().y); // get max y value
        Tile maxTile = floorTileList.Where(tile => tile.GetPosition().y == maxY).First(); // get highest y tile
        
        /* w = wall
         * s = spawnpoint
         * - = floor
         * x = maxTile
         * 
         * w w w
         * w s w
         * w - w w
         * w - - w
         * w w - w
         *     x  
         */
        
        Tile t;
        t = new Tile(new Vector2(maxTile.GetPosition().x, maxTile.GetPosition().y + 1));
        floorTileList.Add(t);
        t = new Tile(new Vector2(maxTile.GetPosition().x, maxTile.GetPosition().y + 2));
        floorTileList.Add(t);
        t = new Tile(new Vector2(maxTile.GetPosition().x - 1, maxTile.GetPosition().y + 2));
        floorTileList.Add(t);
        t = new Tile(new Vector2(maxTile.GetPosition().x - 1, maxTile.GetPosition().y + 3));
        floorTileList.Add(t);
        t = new Tile(new Vector2(maxTile.GetPosition().x - 1, maxTile.GetPosition().y + 4));
        floorTileList.Add(t);
        
        // add floor pieces IJ style generation
        m[(int)maxTile.GetPosition().x, (int)maxTile.GetPosition().y + 1] = 3;
        m[(int)maxTile.GetPosition().x, (int)maxTile.GetPosition().y + 2] = 3;
        m[(int)maxTile.GetPosition().x - 1, (int)maxTile.GetPosition().y + 2] = 3;
        m[(int)maxTile.GetPosition().x - 1, (int)maxTile.GetPosition().y + 3] = 3;
        m[(int)maxTile.GetPosition().x - 1, (int)maxTile.GetPosition().y + 4] = 3;
        
        // add wall pieces on back row
        m[(int)maxTile.GetPosition().x - 2, (int)maxTile.GetPosition().y + 5] = 1;
        m[(int)maxTile.GetPosition().x - 1, (int)maxTile.GetPosition().y + 5] = 1;
        m[(int)maxTile.GetPosition().x, (int)maxTile.GetPosition().y + 5] = 1;
        playerSpawn = t; // set player spawn to last tile
    }
	
    void SpawnEnemies()
    {
        int mobcount = 0;
        foreach (Tile t in floorTileList) {
            //if (t.GetPosition() != treasureSpawn.GetPosition() && FarFromPlayerSpawn(t.GetPosition())) {
            if (FarFromPlayerSpawn(t.GetPosition())) {
                if (Random.Range(0, 10) <= 0) {
                    if (Random.Range(0, 4) == 1) {
                        if (Random.Range(0, 2) == 1)
                            InstantiateObject(enemy1, t.GetPosition());
                        else
                            InstantiateObject(enemy2, t.GetPosition());
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
        GameObject.FindWithTag("Global").GetComponent<GameController>().SetMobCount(mobcount);
    }
    
    void GenerateStartPoint()
    {
        Tile t = new Tile(Vector2.zero);
        floorTileList.Add(t);
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
	
    bool FarFromPlayerSpawn(Vector2 pos)
    {
        return Vector2.Distance(pos, playerSpawn.GetPosition()) > 5;
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
