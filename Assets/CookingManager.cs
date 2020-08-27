using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CookingManager : MonoBehaviour
{
    const float cookTime = 25;

    public AudioManager audioManager;
    public Tilemap tilemap;
    public TileDefs tileDefs;

    public Dictionary<Vector3Int, float> meats = new Dictionary<Vector3Int, float>(); // Dictionary that maps meat position to current cooking time.

    // Update is called once per frame
    void Update()
    {
        foreach (Vector3Int pos in new List<Vector3Int>(meats.Keys))
        {
            // Increase timer for cooked meat
            meats[pos] += Time.deltaTime;

            // When meat is done cooking at a certain position, replace tile at that position with cooked meat.
            if (meats[pos] > cookTime)
            {
                meats.Remove(pos);
                tilemap.SetTile(pos, tileDefs.cookedMeatTile);
                audioManager.PlaySound(audioManager.cook, pos);
            }

            // Remove meat positions from list that are missing or not next to campfire.
            bool nextToCampfire = false;
            for (int x = pos.x - 1; x <= pos.x + 1; x++)
            {
                for (int y = pos.y - 1; y <= pos.y + 1; y++)
                {
                    if (tilemap.GetTile(new Vector3Int(x, y, 0)) == tileDefs.campfireTile)
                    {
                        nextToCampfire = true;
                        break;
                    }
                }
            }
            if (tileDefs.rawMeatTile != tilemap.GetTile(pos) || !nextToCampfire)
            {
                meats.Remove(pos);
            }

        }
    }
}
