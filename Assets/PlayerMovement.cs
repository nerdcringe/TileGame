using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : CharacterMovement
{
    public const float initialSpeed = 5f;
    const float holdEndDelay = 0.1f;

    public Text coordinates;
    public AudioManager audioManager;
    public List<Vector3Int> openedDoorPos;

    public float lastRightTime = 0;
    public float lastLeftTime = 0;
    public float lastUpTime = 0;
    public float lastDownTime = 0;

    // Centers of the edges the player sprite.
    public Vector3[] edges =
    {
        new Vector3( 0.5f, 0),
        new Vector3(-0.5f, 0),
        new Vector3( 0,     0.5f),
        new Vector3( 0,    -0.5f),
    };

    // Start is called before the first frame update
    protected override void Start()
    {
        transform.position = new Vector2(NoiseGen.width / 2, NoiseGen.height / 2);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, 0);
        speed = initialSpeed;
        openedDoorPos = new List<Vector3Int>();
        base.Start();
    }


    bool CheckDoor(bool addToList)
    {
        foreach (Vector3 point in edges)
        {
            Vector3 hitPos = new Vector3(transform.position.x + point.x*1.3f,// + (dir.normalized.x * checkDist),
                                         transform.position.y + point.y*1.3f);// + (dir.normalized.y * checkDist), 0);
            Vector3Int tilePos = Vector3Int.RoundToInt(hitPos);
            //Debug.DrawLine(hitPos, tilePos);
            if (tileDefs.doorTile.Equals(FGTilemap.GetTile(tilePos)) || tileDefs.openDoorTile.Equals(FGTilemap.GetTile(tilePos)))
            {
                //print(hitPos + "  " + (dir.normalized * checkDist) + "  " + tilePos);
                if (!openedDoorPos.Contains(tilePos) && addToList)
                {
                    openedDoorPos.Add(tilePos);
                }
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Vector3Int pos = Vector3Int.RoundToInt(new Vector3(transform.position.x,
                                                           transform.position.y, 0));
        CheckDoor(true);
        // Open door if player is around door.
        for (int i = 0; i < openedDoorPos.Count; i++)
        {
            Vector3Int doorPos = openedDoorPos[i];
            TileBase tile = FGTilemap.GetTile(doorPos);

            // Check if player is still next to or inside door
            if (CheckDoor(false) || doorPos.Equals(pos))
            {
                // Open door in door list if player is next to or inside door
                if (tileDefs.doorTile.Equals(tile))
                {
                    FGTilemap.SetTile(doorPos, tileDefs.openDoorTile);
                    audioManager.PlaySound(audioManager.doorOpened, doorPos);
                }
            }
            else
            {  // Close door in list if player is not next to or inside door
                if (tileDefs.openDoorTile.Equals(tile))
                {
                    FGTilemap.SetTile(doorPos, tileDefs.doorTile);
                    audioManager.PlaySound(audioManager.doorClosed, doorPos);
                }
                openedDoorPos.Remove(doorPos);
            }
        }

        base.Update();
        if (Input.GetKey("d"))
        {
            lastRightTime = Time.time;
        }
        if (Input.GetKey("a"))
        {
            lastLeftTime = Time.time;
        }
        if (Input.GetKey("w"))
        {
            lastUpTime = Time.time;
        }
        if (Input.GetKey("s"))
        {
            lastDownTime = Time.time;
        }

        roundToTargetX = true;
        roundToTargetY = true;

        if (Time.time - lastRightTime < holdEndDelay)
        {
            dir.x = 1;
            roundToTargetX = false;
        }
        if (Time.time - lastLeftTime < holdEndDelay)
        {
            dir.x = -1;
            roundToTargetX = false;
        }

        if (Time.time - lastUpTime < holdEndDelay)
        {
            dir.y = 1;
            roundToTargetY = false;
        }
        if (Time.time - lastDownTime < holdEndDelay)
        {
            dir.y = -1;
            roundToTargetY = false;
        }

        coordinates.text = "(" + Mathf.RoundToInt(transform.position.x) + ", " + Mathf.RoundToInt(transform.position.y) + ")";
    }
}