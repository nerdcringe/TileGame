using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : CharacterMovement
{
    public AudioManager audioManager;
    List<Vector3Int> openedDoorPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        transform.position = new Vector2(NoiseGen.width/2, NoiseGen.height/2);
        speed = 5f;
        openedDoorPos = new List<Vector3Int>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        // Open door if target position is door or is in door.
        Vector3Int pos = new Vector3Int(x, y, 0);

        if (tileDefs.doorTile.Equals(FGTilemap.GetTile(targetPos)) && !openedDoorPos.Equals(targetPos))
        {
            openedDoorPos.Add(targetPos);
        }
        if (tileDefs.doorTile.Equals(FGTilemap.GetTile(pos)) && !openedDoorPos.Equals(pos))
        {
            openedDoorPos.Add(pos);
        }

        for (int i = 0; i < openedDoorPos.Count; i++)
        {
            Vector3Int doorPos = openedDoorPos[i];
            TileBase tile = FGTilemap.GetTile(doorPos);

            if (doorPos.Equals(targetPos) || doorPos.Equals(pos)) {
                // Open door in door list if player position or target position is door position.
                if (tileDefs.doorTile.Equals(tile))
                {
                    FGTilemap.SetTile(doorPos, tileDefs.openDoorTile);
                    audioManager.PlaySound(audioManager.doorOpened, doorPos);
                }
            }
            else
            {  // Close door in list if player is not on or targeting door position.
                if (tileDefs.openDoorTile.Equals(tile))
                {   
                    FGTilemap.SetTile(doorPos, tileDefs.doorTile);
                    audioManager.PlaySound(audioManager.doorClosed, doorPos);
                }
                openedDoorPos.Remove(doorPos);
            }
        }

        // If velocity is less than certain amount, snap targetPos to rounded position.
        if (Mathf.Abs(rb.velocity.x) < 0.005)
        {
            targetPos.x = Mathf.RoundToInt(transform.position.x);
        }
        if (Mathf.Abs(rb.velocity.y) < 0.005)
        {
            targetPos.y = Mathf.RoundToInt(transform.position.y);
        }
        base.Update();

        bool right = Input.GetKey("d");
        bool left = Input.GetKey("a");
        bool up = Input.GetKey("w");
        bool down = Input.GetKey("s");

        roundToTargetPos = true;
        if (right)
        {
            vel.x = 1;
            roundToTargetPos = false;
        }
        if (left)
        {
            vel.x = -1;
            roundToTargetPos = false;
        }
        if (up)
        {
            vel.y = 1;
            roundToTargetPos = false;
        }
        if (down)
        {
            vel.y = -1;
            roundToTargetPos = false;
        }

        if (!right && !left && (up || down))
        {
            vel.x = 0;
        }
        if (!up && !down && (right || left))
        {
            vel.y = 0;
        }
    }
}
