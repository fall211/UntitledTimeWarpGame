using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap resourceTilemap;
    [SerializeField] private Tile[] resourceTiles;
    [SerializeField] private Tilemap obstacleTilemap;
    [SerializeField] private Tile[] obstacleTiles;
    [SerializeField] private Tilemap worldTilemap;
    [SerializeField] private Tile[] worldTiles;
    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private Tile backgroundTile;


    public void GenerateMap(int mapSize, List<List<float>> noiseMap, List<List<float>> falloffMap)
    {   
        GenerateBackgroundTiles(mapSize);
        GenerateWorldTiles(mapSize, noiseMap, falloffMap);
        GenerateResourceTiles(mapSize);
        GenerateObstacleTiles(mapSize);
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

    private void GenerateWorldTiles(int mapSize, List<List<float>> noiseMap, List<List<float>> falloffMap)
    {
        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                float height = Noise.GenYCoords(x+mapSize, y+mapSize, noiseMap, falloffMap);
                if (height > 0.75f)
                {
                    worldTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[1]);
                } else if (height > 0.25){
                    worldTilemap.SetTile(new Vector3Int(x, y, 0), worldTiles[0]);
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
                bool isValidSpawnLocation = worldTilemap.GetTile(new Vector3Int(x, y, 0)) != null;
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

    private void GenerateObstacleTiles(int mapSize){
        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                bool isValidSpawnLocation = worldTilemap.GetTile(new Vector3Int(x, y, 0)) != null && resourceTilemap.GetTile(new Vector3Int(x, y, 0)) == null;
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

    public void ClearMap()
    {
        worldTilemap.ClearAllTiles();
        resourceTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();
        obstacleTilemap.ClearAllTiles();
    }
}
