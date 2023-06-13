using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ResourceTile
{
    public Vector3Int tilePosition; 
    public TileBase tile;
    public int amount; // amount of resource

    public ResourceTile(Vector3Int pos, TileBase tile, int amount)
    {
        tilePosition = pos;
        this.tile = tile;
        this.amount = amount;
    }
}

