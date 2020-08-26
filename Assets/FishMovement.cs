using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : CharacterMovement
{
    const int despawnDistance = 40;

    public Transform player;
    SpriteRenderer sr;

    float angle = 0;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {

        base.Update();
        bool targetingWater = tileDefs.waterTile.Equals(FGTilemap.GetTile(targetPos));

        // Swim quickly in water and slow out of water
        if (tileDefs.waterTile.Equals(FGTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0))))
        {
            speed = 4.5f;
            if (!targetingWater)
            {
                angle += 180;
            }
        }
        else
        {
            speed = 0.25f;
        }

        if (Random.Range(0, 120) < 1)
        {
            angle = Random.Range(0.0f, 360.0f);
        }

        if (Vector3.Distance(player.position, transform.position) > despawnDistance)
        {
            Destroy(gameObject);
        }

        vel = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        vel.Normalize();
        vel *= speed;
        vel = Vector2.ClampMagnitude(vel, speed);

        if (vel.x > 0 && vel.y < 0)
        {
            sr.flipX = false;
            sr.flipY = false;
            transform.eulerAngles = new Vector3Int(0, 0, 0);
        }
        if (vel.x < 0 && vel.y < 0)
        {
            sr.flipX = true;
            sr.flipY = false;
            transform.eulerAngles = new Vector3Int(0, 0, 0);
        }
        if (vel.x > 0 && vel.y > 0)
        {
            sr.flipX = false;
            sr.flipY = false;
            transform.eulerAngles = new Vector3Int(0, 0, 90);
        }
        if (vel.x < 0 && vel.y > 0)
        {
            sr.flipX = false;
            sr.flipY = true;
            transform.eulerAngles = new Vector3Int(0, 0, 90);
        }
    }
}
