using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public Vector2 vel;
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
        targetPos = new Vector3Int(Mathf.RoundToInt(transform.position.x + vel.x), Mathf.RoundToInt(transform.position.y + vel.y), 0);
        targetPos = NoiseGen.ClampComponentsInside(targetPos);
        transform.position = NoiseGen.ClampComponentsInside(transform.position);
        vel = Vector2.zero;
    }

    void FixedUpdate()
    {
        Vector2 dir = vel;
        
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
        dir.Normalize();
        dir *= currentSpeed;

        if (roundToTargetX)
        {
            dir.x = targetPos.x - rb.position.x;
            dir.x /= Time.fixedDeltaTime;
        }
        if (roundToTargetY)
        {
            dir.y = targetPos.y - rb.position.y;
            dir.y /= Time.fixedDeltaTime;
        }

        dir = Vector2.ClampMagnitude(dir, currentSpeed);
        rb.velocity = dir;
    }
}
