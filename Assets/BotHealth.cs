using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BotHealth : MonoBehaviour
{
    public CharacterMovement mov;
    public TileDefs tileDefs;
    public Tilemap tilemap;

    public void Cease()
    {
        TileBase dropTile = tileDefs.steelTile;
        if (Random.Range(0, 12) < 1)
        {
            dropTile = tileDefs.circutBoardTile;
        }
        tilemap.SetTile(new Vector3Int(Mathf.RoundToInt(mov.transform.position.x), Mathf.RoundToInt(mov.transform.position.y), 0), dropTile);
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int tilePos = new Vector3Int(Mathf.RoundToInt(mov.transform.position.x), Mathf.RoundToInt(mov.transform.position.y), 0);
        if (tileDefs.magmaTile.Equals(tilemap.GetTile(tilePos)))
        {
            Cease();
        }
    }
}
