using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileType
{
    const float volume = 1.0f;

    public readonly int id;
    public readonly string name;
    public readonly Tile tile;

    public readonly float gatherTime; // Time for player to gather tile (seconds).
    public readonly float breakTime; // Time for enemies to break tile.
    
    public readonly Sprite sprite;
    readonly AudioClip sound; // Sound that occurs when tile is gathered or broked by robot.

    public TileType(int ID, string Name, Tile Tile, float GatherTime, float BreakTime, AudioClip Sound)
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
        sound = Sound;
    }

    public void PlaySound(AudioSource audioSource)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound, volume);
        }
    }

}
