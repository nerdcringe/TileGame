using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BotMovement : CharacterMovement
{
    const int despawnDistance = 40;

    public Transform player;
    public AudioManager audioManager;

    public Vector3Int breakPos;
    public float breakTime = 0;

    void BreakTiles()
    {
        // If any vector from 4 corners of bot collider in direction of velocity hit a tile, then break the tiles corner by corner.
        if (breakTime == 0)
        {
            List<Vector3> cornerPos = new List<Vector3>();

            cornerPos.Add(transform.position + new Vector3(0.5f, 0.5f, 0));
            cornerPos.Add(transform.position + new Vector3(0.5f, -.5f, 0));
            cornerPos.Add(transform.position + new Vector3(-.5f, -.5f, 0));
            cornerPos.Add(transform.position + new Vector3(-.5f, 0.5f, 0));
            float dirMultiplier = 0.05f;

            bool breakingTile = false;
            foreach (Vector3 pos in cornerPos)
            {
                Vector3Int tilePos = new Vector3Int(Mathf.RoundToInt(pos.x + (dir.x * dirMultiplier)), Mathf.RoundToInt(pos.y + (dir.y * dirMultiplier)), 0); 
                if (FGTilemap.GetTile(tilePos) != null)
                {
                    breakPos = tilePos;
                    breakingTile = true;
                    break;
                }
            }
            if (!breakingTile)
            {
                breakTime = 0;
                breakPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            }
        }

        TileBase tile = FGTilemap.GetTile(breakPos);

        if (tile != null)
        {
            breakTime += Time.deltaTime;

            TileType tileType = tileDefs.GetTileFromName(tile.name);
            if (breakTime > tileType.breakTime)
            {
                FGTilemap.SetTile(breakPos, null);
                audioManager.PlaySound(tileType.sound, breakPos);
                breakTime = 0;
            }
        }
        else
        {
            breakTime = 0;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        dir = player.position - transform.position;
        dir = dir.normalized;

        BreakTiles();

        if (Vector3.Distance(player.position, transform.position) > despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}