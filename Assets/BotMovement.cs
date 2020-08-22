using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BotMovement : CharacterMovement
{
    const int despawnDistance = 40;

    public Transform player;

    public Vector3Int tilePos;
    public float breakTime = 0;

    void BreakTiles()
    {
        // If far enough away from tile position, stop targeting it
        if (Vector3.Distance(transform.position, tilePos) > 1.5)
        {
            breakTime = 0;
        }

        if (breakTime == 0)
        {
            tilePos = targetPos;
            Vector3Int side1 = new Vector3Int(Mathf.RoundToInt(transform.position.x), tilePos.y, 0);
            Vector3Int side2 = new Vector3Int(tilePos.x, Mathf.RoundToInt(transform.position.y), 0);

            // Don't break diagonally if both tiles next to diagonal are not empty.
            if (FGTilemap.GetTile(side1) != null || FGTilemap.GetTile(side2) != null)
            {
                // Choose either side to break.
                if (Random.Range(0, 2) == 1)
                {
                    tilePos = side1;
                }
                else
                {
                    tilePos = side2;
                }
            }
        }

        TileBase tile = FGTilemap.GetTile(tilePos);
        if (tile != null)
        {
            breakTime += Time.deltaTime;

            if (breakTime > tileDefs.GetTileFromName(tile.name).breakTime)
            {
                FGTilemap.SetTile(tilePos, null);
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
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        base.Update();

        if (player.position.x > x)
        {
            targetPos.x = x + 1;
        }
        if (player.position.x < x)
        {
            targetPos.x = x - 1;
        }
        if (player.position.y > y)
        {
            targetPos.y = y + 1;
        }
        if (player.position.y < y)
        {
            targetPos.y = y - 1;
        }
        BreakTiles();

        if (Vector3.Distance(player.position, transform.position) > despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}