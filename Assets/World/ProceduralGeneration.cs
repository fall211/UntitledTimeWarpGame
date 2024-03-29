using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap resourceTilemap;
    [SerializeField] private Tile[] resourceTiles;
    [SerializeField] private Tile[] treeTiles;
    [SerializeField] private Tile[] rockTiles;
    [SerializeField] private Tilemap obstacleTilemap;
    [SerializeField] private Tile[] obstacleTiles;
    [SerializeField] private GameObject world;
    [SerializeField] private Tilemap worldTilemap;
    [SerializeField] private RuleTile[] worldTiles;
    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private Tile backgroundTile;


    public void GenerateMap(int mapSize,
        List<List<float>> heightMap
        )
    {   
        GenerateBackgroundTiles(mapSize);
        GenerateWorldTiles(mapSize, heightMap);

        GenerateResourceBiomes(mapSize, heightMap);

        CleanUpWorldTiles(mapSize);
        // GenerateResourceTiles(mapSize);
        // GenerateObstacleTiles(mapSize);
    }

    private void GenerateBackgroundTiles(int mapSize)
    {
        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), backgroundTile);
            }
        }
    }

    private void GenerateWorldTiles(int mapSize, List<List<float>> heightMap)
    {
        Tilemap grassTilemap = world.transform.Find("Grass").GetComponent<Tilemap>();
        Tilemap forestTilemap = world.transform.Find("Forest").GetComponent<Tilemap>();
        Tilemap sandTilemap = world.transform.Find("Sand").GetComponent<Tilemap>();
        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                float height = heightMap[x + mapSize][y + mapSize];
                if (height > 0.25f)
                {
                    // sand
                    sandTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[2]);
                    worldTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[2]);
                }
                if (height > 0.35f)
                {
                    // grass
                    grassTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[0]);
                    worldTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[0]);
                }
                if (height > 0.65f){
                    // forest
                    forestTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[1]);
                    worldTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[1]);
                }
            }
        }
    }

    private void GenerateResourceBiomes(int mapSize, List<List<float>> heightMap){
        ResourceManager resourceManager = resourceTilemap.GetComponent<ResourceManager>();

        for (int x = -mapSize; x < mapSize; x++){
            for (int y = -mapSize; y < mapSize; y++){
                float height = heightMap[x + mapSize][y + mapSize];
                bool isValidSpawnLocation = ValidSpawnLocation(x, y, mapSize) && resourceTilemap.GetTile(new Vector3Int(x, y, 0)) == null;

                if (height > 0.60f){
                    bool isResourceTile = Random.Range(0, 100) < 10;
                    if (isResourceTile && isValidSpawnLocation)
                    {
                        int randomTileIndex = Random.Range(0, treeTiles.Length);
                        resourceManager.AddResourceTile(new Vector3Int(x, y, 0), treeTiles[randomTileIndex]);
                    }
                } else if (height < 0.40f && height > 0.25f)
                {
                    bool isResourceTile = Random.Range(0, 100) < 10;

                    if (isResourceTile && isValidSpawnLocation)
                    {
                        int randomTileIndex = Random.Range(0, rockTiles.Length);
                        resourceManager.AddResourceTile(new Vector3Int(x, y, 0), rockTiles[randomTileIndex]);
                    }
                }
            }
        }
        
    }

    private void CleanUpWorldTiles(int mapSize) {
    for (int x = -mapSize; x < mapSize; x++)
    {
        for (int y = -mapSize; y < mapSize; y++)
        {
            Vector3Int tilePosition = new Vector3Int(x, y, 0);
            TileBase tile = worldTilemap.GetTile(tilePosition);
            if (tile != null)
            {
                Vector3Int[] adjacentPositions = new Vector3Int[] {
                    new Vector3Int(x + 1, y, 0),
                    new Vector3Int(x - 1, y, 0),
                    new Vector3Int(x, y + 1, 0),
                    new Vector3Int(x, y - 1, 0)
                };
                int adjacentTiles = 0;
                Vector3Int firstAdjacentPosition = Vector3Int.zero;
                foreach (Vector3Int pos in adjacentPositions) {
                    if (worldTilemap.GetTile(pos) != null) {
                        adjacentTiles++;
                        if (adjacentTiles == 1) {
                            firstAdjacentPosition = pos;
                        }
                    }
                }
                if (adjacentTiles == 1) {
                    worldTilemap.SetTile(tilePosition, null);
                }
                else if (adjacentTiles == 2)
                {
                    Vector3Int secondAdjacentPosition = adjacentPositions.First(pos => worldTilemap.GetTile(pos) != null && pos != firstAdjacentPosition);
                    if ((firstAdjacentPosition.x == x + 1 && secondAdjacentPosition.x == x - 1) ||
                        (firstAdjacentPosition.x == x - 1 && secondAdjacentPosition.x == x + 1) ||
                        (firstAdjacentPosition.y == y + 1 && secondAdjacentPosition.y == y - 1) ||
                        (firstAdjacentPosition.y == y - 1 && secondAdjacentPosition.y == y + 1))
                    {
                        worldTilemap.SetTile(tilePosition, null);
                    }
                }
            }
        }
    }
}

    private void GenerateResourceTiles(int mapSize){
        ResourceManager resourceManager = resourceTilemap.GetComponent<ResourceManager>();
        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                // check that it isnt near the edge of the map
                bool isValidSpawnLocation = ValidSpawnLocation(x, y, mapSize) && resourceTilemap.GetTile(new Vector3Int(x, y, 0)) == null;

                if (isValidSpawnLocation)
                {
                    bool isResourceTile = Random.Range(0, 100) < 1;
                    if (isResourceTile)
                    {
                        int randomTileIndex = Random.Range(0, resourceTiles.Length);
                        resourceManager.AddResourceTile(new Vector3Int(x, y, 0), resourceTiles[randomTileIndex]);
                    }

                }

            }
        }
    }

    private bool ValidSpawnLocation(int x, int y, int mapSize){
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }
                Vector3Int pos = new Vector3Int(x + i, y + j, 0);
                if (worldTilemap.GetTile(pos) == null) {
                    return false;
                }
            }
        }
        return true;
    }

    private void GenerateObstacleTiles(int mapSize){
        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                bool isValidSpawnLocation = ValidSpawnLocation(x, y, mapSize) && resourceTilemap.GetTile(new Vector3Int(x, y, 0)) == null;
                if (isValidSpawnLocation)
                {
                    bool isObstacleTile = Random.Range(0, 100) < 1;
                    if (isObstacleTile)
                    {
                        int randomTileIndex = Random.Range(0, obstacleTiles.Length);
                        obstacleTilemap.SetTile(new Vector3Int(x, y, 0), obstacleTiles[randomTileIndex]);
                    }

                }

            }
        }
    }

    private void ClearAllTilemaps()
    {
        world.transform.Find("Grass").GetComponent<Tilemap>().ClearAllTiles();
        world.transform.Find("Forest").GetComponent<Tilemap>().ClearAllTiles();
        world.transform.Find("Sand").GetComponent<Tilemap>().ClearAllTiles();
        worldTilemap.ClearAllTiles();

        resourceTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();
        obstacleTilemap.ClearAllTiles();
    }

    public void ClearMap()
    {
        ClearAllTilemaps();
    }
}
