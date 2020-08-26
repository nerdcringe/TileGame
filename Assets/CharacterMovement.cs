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

    public bool roundToTargetPos = false;

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
        TileBase tile = FGTilemap.GetTile(targetPos);
        if (tile != null)
        {
            if (tile == tileDefs.waterTile && !gameObject.tag.Equals("Fish"))
            {
                currentSpeed *= 0.6f;
            }
            if (tile == tileDefs.magmaTile)
            {
                currentSpeed *= 0.35f;
            }
        }
        if (roundToTargetPos)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.25)
            {
                dir.x = Mathf.RoundToInt(dir.x);
            }
            if (Mathf.Abs(rb.velocity.y) < 0.25)
            {
                dir.y = Mathf.RoundToInt(dir.y);
            }

            dir = new Vector2(targetPos.x, targetPos.y) - rb.position;
        }
        else
        {
            dir.Normalize();
            dir *= currentSpeed;
        }

        dir /= Time.fixedDeltaTime;
        dir = Vector2.ClampMagnitude(dir, currentSpeed);
        rb.velocity = dir;
    }
}
