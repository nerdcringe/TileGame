using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BGTileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileDefs tileDefs;
    
    public Tile GetTileAtValue(float noiseVal)
    {
        Tile tile = tileDefs.grassTile;
        if (noiseVal > 0.62)
        {
            tile = tileDefs.darkRockTile;
        }
        else if (noiseVal < 0.355)
        {
            tile = tileDefs.sandTile;
        }
        return tile;
    }


    // Start is called before the first frame update
    void Start()
    {
        NoiseGen.Generate();

        for (int x = 0; x < NoiseGen.width; x++)
        {
            for (int y = 0; y < NoiseGen.height; y++)
            {
                float noiseVal = NoiseGen.noisemap[x, y];
                Tile tile = GetTileAtValue(noiseVal);
                
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
