using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    private List<ResourceTile> resources = new List<ResourceTile>();



    public ResourceTile GetResourceTile(Vector3Int tilePosition)
    {
        foreach (ResourceTile resource in resources)
        {
            if (resource.tilePosition == tilePosition)
            {
                return resource;
            }
        }

        return null;
    }

    public void GatherResource(Vector3Int tilePosition)
    {
        ResourceTile resource = GetResourceTile(tilePosition);

        if (resource != null)
        {
            resource.amount--;

            if (resource.amount <= 0)
            {
                tilemap.SetTile(resource.tilePosition, null);
                resources.Remove(resource);
            }
        }
    }

    public void AddResourceTile(Vector3Int tilePosition, Tile tile)
    {
        resources.Add(new ResourceTile(tilePosition, tile));
        tilemap.SetTile(tilePosition, tile);
    }
}
