using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public Vector3Int targetPos;

    public Tilemap FGTilemap;
    public TileDefs tileDefs;
    public Rigidbody2D rb;

    public bool roundToTargetX = false;
    public bool roundToTargetY = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        targetPos = new Vector3Int(Mathf.RoundToInt(transform.position.x + dir.x), Mathf.RoundToInt(transform.position.y + dir.y), 0);
        targetPos = NoiseGen.ClampComponentsInside(targetPos);
        transform.position = NoiseGen.ClampComponentsInside(transform.position);
        dir = Vector2.zero;
    }

    void FixedUpdate()
    {
        Vector2 vel = dir;
        
        float currentSpeed = speed;
        TileBase tile = FGTilemap.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0));
        if (tile != null)
        {
            if (tile == tileDefs.waterTile && !gameObject.tag.Equals("Fish"))
            {
                currentSpeed *= 0.65f;
            }
            if (tile == tileDefs.magmaTile)
            {
                currentSpeed *= 0.4f;
            }
        }
        vel.Normalize();
        vel *= currentSpeed;

        if (roundToTargetX)
        {
            vel.x = targetPos.x - rb.position.x;
            vel.x /= Time.fixedDeltaTime;
        }
        if (roundToTargetY)
        {
            vel.y = targetPos.y - rb.position.y;
            vel.y /= Time.fixedDeltaTime;
        }

        vel = Vector2.ClampMagnitude(vel, currentSpeed);
        rb.velocity = vel;
    }
}
