using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishHealth : MonoBehaviour
{
    const int outOfWaterMaxTime = 8;

    public TileDefs tileDefs;
    public Tilemap tilemap;

    float outOfWaterTimer = 0;

    public void Cease()
    {
        tilemap.SetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0), tileDefs.rawMeatTile);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int tilePos = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
        if (tileDefs.magmaTile.Equals(tilemap.GetTile(tilePos)) || outOfWaterTimer > outOfWaterMaxTime)
        {
            Cease();
        }

        // Damage fish when outside of water.
        if (!tileDefs.waterTile.Equals(tilemap.GetTile(tilePos)))
        {
            outOfWaterTimer += Time.deltaTime;
        }
        else
        {
            outOfWaterTimer = 0;
        }
    }
}
