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
    private Tile treasureSpawn;
    private Tile playerSpawn;
	
    private float tileSizeOffset = 1f;
	
   
    void Start()
    {	
        floorTileList = new List<Tile>();
		
        GenerateWorld();
    }
	
    public void GenerateWorld()
    {
        // remove all child objects from world
        foreach (Transform child in gameObject.transform) {
            Destroy(child.gameObject);
        }
        
        // reset world array
        m = new int[H, W];
        
        // reset tile Lists
        floorTileList.Clear();
		
        GenerateCanyons();     // generate world IJ style
        GenerateSpawnArea();
        GenerateCanyonWalls();
        
        GenerateChest();    // only keep one chest
        SpawnEnemies();
        
        // tell game we're done with world gen.
        GlobalParams.MarkWorldGenComplete();
    }
    
    #region IJ CANYON MAP GENERATOR
    
    /* Values:
     * 3 = floor
     * 2 = low wall
     * 1 = mid wall
     * 0 = high wall
     * -1 = trimmed
    */
    
    static int C = 8;
    static int W = 60;
    static int H = 60;
    int[,] m = new int[H, W];
    
    void GenerateCanyons()
    {
        for (int z = 0; z < 4; z++) {
            Zigzag();
            Smooth();
        }
        
        // remove excess high canyon points
        DestroyExcess();
        
        // floors
        for (int x = 0; x < W; x++) {
            for (int y = 0; y < H; y++) {
                if (m[x, y] == 3) {
                    AddToFloorTileList(new Vector2(x, y));
                }
            }
        }
    }
    
    void GenerateCanyonWalls()
    {
        for (int x = 0; x < W; x++) {
            for (int y = 0; y < H; y++) {
                if (m[x, y] == 2) {
                    InstantiateWall(wallTile, new Vector3(x, 0, y));
                } else if (m[x, y] == 1) {
                    InstantiateWall(wallTile, new Vector3(x, 0, y));
                    InstantiateWall(wallTile, new Vector3(x, 1, y));
                } else if (m[x, y] == 0) {
                    InstantiateWall(wallTile, new Vector3(x, 2, y));
                }
            }
        }
    }
    
    void Zigzag()
    {
        int x = C;
        int y = C;
        while (x < W-C-1 || y < H-C-1) {
            int dx = 0;
            int dy = 0;
            if (x == W - C - 1) {
                dy = Random.Range(1, Mathf.Min(H - C - 1 - y, 3));
            } else if (y == H - C - 1) {
                dx = Random.Range(1, Mathf.Min(W - C - 1 - x, 3));
            } else if (Random.Range(0, 2) < 1) {
                dy = Random.Range(1, Mathf.Min(H - C - 1 - y, 3));
            } else {
                dx = Random.Range(1, Mathf.Min(W - C - 1 - x, 3));
            }
            Carve(x, y, dx, dy);
            x += dx;
            y += dy;
        }
        m[W - C - 1, H - C - 1] = 3;
    }
    
    void Carve(int x, int y, int dx, int dy)
    {
        while (dx > 0 || dy > 0) {
            m[x, y] = 3;
            if (Random.Range(0, 10) < 3) {
                for (int i=-1; i<=1; i++) {
                    for (int j=-1; j<=1; j++) {
                        m[x + i, y + j] = 3;
                    }
                }
            }
            if (dx > 0) {
                dx--;
                x++;
            }
            if (dy > 0) {
                dy--;
                y++;
            }
        }
    }
    
    void Smooth()
    {
        for (int x = 0; x < W; x++) {
            for (int y = 0; y < H; y++) {
                if (m[x, y] == 3) {
                    continue;
                }
                if (InRange(x, y, 3, 3)) {
                    m[x, y] = Random.Range(0, 100) < 75 ? 2 : 1;
                } else if (InRange(x, y, 3, 4)) {
                    m[x, y] = 1;
                }
            }
        }
    }
    
    void DestroyExcess()
    {
        for (int x = 0; x < W; x++) {
            for (int y = 0; y < H; y++) {
                if (m[x, y] != 0) {
                    continue;
                }
                if (NeedsTrim(x, y)) {
                    m[x, y] = -1;
                }
            }
        }
    }
    
    bool NeedsTrim(int x, int y)
    {
        for (int a = -1; a <= 1; a++) {
            for (int b = -1; b <= 1; b++) {
                int t = Get(x + a, y + b);
                if (t == 1 || t == 2) {
                    return false;
                }
            }
        }
        return true;
    }

    bool InRange(int x, int y, int val, int range)
    {
        for (int a = -range; a <= range; a++) {
            for (int b = -range; b <= range; b++) {
                if (Get(x + a, y + b) == val) {
                    return true;
                }
            }
        }
        return false;
    }

    int Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= W || y >= H) {
            return 0;
        }
        return m[x, y];
    }
    
    
    #endregion
    
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
            if (t.GetPosition() != treasureSpawn.GetPosition() && FarFromPlayerSpawn(t.GetPosition())) {
                if (Random.Range(0, 3) <= 0) {
                    if (Random.Range(0, 4) <= 1) {
                        if (Random.Range(0, 2) == 1)
                            InstantiateEnemy(enemy1, t.GetPosition());
                        else
                            InstantiateEnemy(enemy2, t.GetPosition());
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
	
    public Vector2 getPlayerSpawnPosition()
    {
        return playerSpawn.GetPosition();
    }
	
    void GenerateChest()
    {
        var minX = floorTileList.Min(tile => tile.GetPosition().x); // get min value
        Tile minTile = floorTileList.Where(tile => tile.GetPosition().x == minX).Last(); // get lowest x tile
		
        // set treasure spawn
        treasureSpawn = minTile;
        
        // get proper rotation so that chest always faces open tile
        Quaternion rotation = GetChestRotationFacingOpenTile(treasureSpawn.GetPosition());
        
        // create the chest
        InstantiateTile(treasureTile, new Vector3(treasureSpawn.GetPosition().x, treasureTile.transform.position.y, treasureSpawn.GetPosition().y), rotation);
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
	
    // 3 for floor, 2 for lowest wall
    void AddToFloorTileList(Vector2 position)
    {
        // add to floor list
        if (!TileExistsWithPosition(position, floorTileList)) {
            Tile t = new Tile(position);
            floorTileList.Add(t);
        }
    }
    
    void InstantiateEnemy(GameObject obj, Vector2 position)
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
    
    void InstantiateWall(GameObject tile, Vector3 position)
    {
        // instantiate wall prefab in unity and set it as child object to World
        GameObject newWall = (GameObject)Instantiate(tile, new Vector3(position.x * tileSizeOffset, position.y, position.z * tileSizeOffset), Quaternion.identity);
        newWall.transform.parent = gameObject.transform;
        
        // set proper texture based on y position
        if (position.y == 0) {
            MakeBottomWallTexture(newWall);
        }
        
        // destroy quads that aren't facing open area that player might see and destroy wall if all quads gone
        if (DisableNonVisibleQuads(newWall, position)) {
            Destroy(newWall);
        }
    }
    
    void MakeBottomWallTexture(GameObject wall)
    {
        Texture sandyWall = wall.GetComponent<MeshRenderer>().materials[1].mainTexture;
        for (int i = 0; i < wall.transform.childCount; i++) {
            MeshRenderer[] childRenderers = wall.transform.GetChild(i).GetComponents<MeshRenderer>();
            for (int j = 0; j < childRenderers.Count(); j++) {
                // swap material texture for sandywall
                childRenderers[j].material.mainTexture = sandyWall;
            }
        }
    }
    
    // returns true if all quads were destroyed
    bool DisableNonVisibleQuads(GameObject wall, Vector3 position)
    {
        // get x,y location on map array
        int x = (int)position.x;
        int y = (int)position.z;
        int value = m[x, y];
        
        List<Transform> deadChildList = new List<Transform>();
        
        // south 0
        if (y - 1 >= 0 && m[x, y - 1] <= value) {
            deadChildList.Add(wall.transform.GetChild(0));
        }
        // east  1
        if (x + 1 < W && m[x + 1, y] <= value) {
            deadChildList.Add(wall.transform.GetChild(1));
        }
        // north 2
        if (y + 1 < H && m[x, y + 1] <= value) {
            deadChildList.Add(wall.transform.GetChild(2));
        }
        // west  3
        if (x - 1 >= 0 && m[x - 1, y] <= value) {
            deadChildList.Add(wall.transform.GetChild(3));
        }
        
        deadChildList.ForEach(child => Destroy(child.gameObject));
        return (deadChildList.Count == 4);
    }
    
    bool TileExistsWithPosition(Vector2 position, List<Tile> tileGroup)
    {
        var matches = tileGroup.Where(x => x.GetPosition() == position);
        return (matches.Count() > 0);
    }
    
    bool FarFromPlayerSpawn(Vector2 pos)
    {
        return Vector2.Distance(pos, playerSpawn.GetPosition()) > 5;
    }
}
