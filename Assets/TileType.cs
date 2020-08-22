using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileType
{
    public readonly int id;
    public readonly string name;
    public readonly Tile tile;

    public readonly float gatherTime; // Time for player to gather tile (seconds).
    public readonly float breakTime; // Time for enemies to break tile.
    
    public readonly Sprite sprite;

    public TileType(int ID, string Name, Tile Tile, float GatherTime, float BreakTime)
    {
        id = ID;
        name = Name;
        tile = Tile;
        tile.name = name;

        gatherTime = GatherTime;
        breakTime = BreakTime;

        if (tile is AnimatedTile)
        {
            sprite = ((AnimatedTile)tile).m_AnimatedSprites[0];
        }
        else
        {
            sprite = tile.sprite;
        } 
    }
}
