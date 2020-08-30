using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FGTileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileDefs tileDefs;

    public Transform player;

    public void Tree(int x, int y)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), tileDefs.logTile);
        Vector3Int[] leafTiles = new Vector3Int[4];

        leafTiles[0] = new Vector3Int(x + 1, y, 0);
        leafTiles[1] = new Vector3Int(x - 1, y, 0);
        leafTiles[2] = new Vector3Int(x, y + 1, 0);
        leafTiles[3] = new Vector3Int(x, y - 1, 0);

        foreach (Vector3Int pos in leafTiles)
        {
            if (tilemap.GetTile(pos) == null)
            {
                tilemap.SetTile(pos, tileDefs.leafTile);
            }
        }
    }

    // Start is called before the first frame update
    public void Generate()
    {
        for (int x = 0; x < NoiseGen.width; x++)
        {
            for (int y = 0; y < NoiseGen.height; y++)
            {
                float noiseVal = NoiseGen.noisemap[x, y];
                if (noiseVal < 0.326)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileDefs.waterTile);
                }
                else
                {
                    // Cave Generation
                    if (noiseVal > 0.61)
                    {
                        if (noiseVal > 0.72 && Random.Range(0, 135) < 1)
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), tileDefs.steelTile);
                        }
                        else if (noiseVal > 0.66 && Random.Range(0, (1-noiseVal) * 36) < 1)
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), tileDefs.magmaTile);
                        }
                        else if (noiseVal < 0.625 || Random.Range(0, 3) < 1)
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), tileDefs.rockTile);
                        }
                    }
                    else if (noiseVal > 0.38 && noiseVal < 0.56)
                    {
                        if (Random.Range(0, 33) < 1)
                        {
                            Tree(x, y);
                        }
                    }
                }
            }
        }

        tilemap.SetTile(new Vector3Int(NoiseGen.width/2, NoiseGen.height/2, 0), null);
    }
}
