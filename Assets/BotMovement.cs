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
        // If far enough away from tile position, stop targeting it.
        if (Vector3.Distance(transform.position, breakPos) > 1.2f)
        {
            breakTime = 0;
        }

        if (breakTime == 0)
        {
            //Vector3Int currrentPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            //Vector3Int roundedVel = new Vector3Int(Mathf.RoundToInt(vel.x), Mathf.RoundToInt(vel.y), 0);

            List<Vector3> cornerPos = new List<Vector3>();

            cornerPos.Add(transform.position + new Vector3(0.5f, 0.5f, 0));
            cornerPos.Add(transform.position + new Vector3(0.5f, -.5f, 0));
            cornerPos.Add(transform.position + new Vector3(-.5f, 0.5f, 0));
            cornerPos.Add(transform.position + new Vector3(-.5f, -.5f, 0));

            foreach (Vector3 pos in cornerPos)
            {
                Vector3Int tilePos = new Vector3Int(Mathf.RoundToInt(pos.x + vel.x), Mathf.RoundToInt(pos.y + vel.y), 0);

                if (FGTilemap.GetTile(tilePos) != null)
                {
                    breakPos = tilePos;
                }
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
        vel = player.position - transform.position;
        vel = vel.normalized;

        BreakTiles();

        if (Vector3.Distance(player.position, transform.position) > despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}