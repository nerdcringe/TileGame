using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishSpawning : MonoBehaviour
{
    const int spawnInterval = 10;
    const int maxFish = 3;

    const int minSpawnRadius = 16;
    const int maxSpawnRadius = 22;

    public TileDefs tileDefs;
    public Tilemap tilemap;
    public GameObject fishFab;
    public Transform player;

    float spawnTimer = 0;


    public void SpawnFish(Vector3 spawnPos)
    {
        GameObject fish = Instantiate(fishFab, gameObject.transform);
        fish.transform.position = spawnPos;
        FishMovement ai = fish.GetComponent<FishMovement>();
        ai.player = player;
        ai.FGTilemap = tilemap;
        ai.tileDefs = tileDefs;

        FishHealth health = fish.GetComponent<FishHealth>();
        health.tilemap = tilemap;
        health.tileDefs = tileDefs;
    }

    // Start is called before the first frame update
    void Update()
    {
        if (spawnTimer > spawnInterval && GameObject.FindGameObjectsWithTag("Fish").Length < maxFish)
        {
            Vector3Int spawnPos;
            for (int i = 0; i < 5; i++) // Attempt to spawn around player several times.
            {

                float angle = Random.Range(0, Mathf.PI * 2);
                Vector3Int posAroundPlayer = new Vector3Int(Mathf.RoundToInt(Mathf.Cos(angle)), Mathf.RoundToInt(Mathf.Sin(angle)), 0);
                posAroundPlayer *= Random.Range(minSpawnRadius, maxSpawnRadius);

                spawnPos = new Vector3Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y), 0) + posAroundPlayer;
                SpawnFish(spawnPos);

                if (tileDefs.waterTile.Equals(tilemap.GetTile(spawnPos)))
                {
                    spawnTimer = 0;
                    break;
                }
            }

        }
        spawnTimer += Time.deltaTime;
    }
}
