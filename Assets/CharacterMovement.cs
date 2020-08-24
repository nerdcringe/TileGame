using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public Vector3Int targetPos;

    public Tilemap FGTilemap;
    public TileDefs tileDefs;
    public Rigidbody2D rb;
    protected bool roundToNearestPos = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // If velocity is less than certain amount, snap targetPos to rounded position.
        if (Mathf.Abs(rb.velocity.x) < 0.005)
        {
            targetPos.x = Mathf.RoundToInt(transform.position.x);
        }
        if (Mathf.Abs(rb.velocity.y) < 0.005)
        {
            targetPos.y = Mathf.RoundToInt(transform.position.y);
        }

        targetPos = NoiseGen.ClampComponentsInside(targetPos);
        transform.position = NoiseGen.ClampComponentsInside(transform.position);
    }

    void FixedUpdate()
    {
        Vector2 dir;
        dir = new Vector2(targetPos.x, targetPos.y) - rb.position;

        if (!roundToNearestPos)
        {
            dir.Normalize();
            dir *= speed;
        }
        dir /= Time.fixedDeltaTime;

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
        dir = Vector2.ClampMagnitude(dir, currentSpeed);
        rb.velocity = dir;
    }
}
