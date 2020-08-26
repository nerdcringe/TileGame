using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class BotSpawning : MonoBehaviour
{
    const int spawnRadius = 16;
    const float maxBots = 3;
    const float spawnIntervalMin = 18;
    const float spawnIntervalMax = 30;

    public GameObject botFab;
    public Tilemap FGTilemap;
    public Tilemap BGTilemap;
    public TileDefs tileDefs;

    public Transform player;
    public AudioManager audioManager;

    public float currentSpawnInterval = 34;
    float spawnTimer = 0;

    float currentSpeed = 3.5f;

    public void SpawnBot(Vector3 spawnPos)
    {
        GameObject bot = Instantiate(botFab, transform);
        bot.transform.position = spawnPos;

        BotMovement ai = bot.GetComponent<BotMovement>();
        ai.speed = currentSpeed;
        ai.player = player;
        ai.FGTilemap = FGTilemap;
        ai.tileDefs = tileDefs;
        ai.audioManager = audioManager;

        BotHealth health = bot.GetComponent<BotHealth>();
        health.tilemap = FGTilemap;
        health.tileDefs = tileDefs;
    }


    // Update is called once per frame
    void Update()
    {
        int numBots = GameObject.FindGameObjectsWithTag("Enemy").Length;
        bool botsMaxedOut = numBots >= maxBots;

        if (spawnTimer > currentSpawnInterval && !botsMaxedOut)
        {
            Vector3Int spawnPos;

            // Make sure bot's position around player is not on a foreground tile.
            for (int i = 0; i < 5; i++) // Attempt to spawn around player several times.
            {
                float angle = Random.Range(0, Mathf.PI * 2);
                Vector3Int posAroundPlayer = new Vector3Int(Mathf.RoundToInt(Mathf.Cos(angle)), Mathf.RoundToInt(Mathf.Sin(angle)), 0);

                posAroundPlayer *= spawnRadius;
                spawnPos = new Vector3Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y), 0) + posAroundPlayer;

                // Don't spawn in lamp light range.
                int lampRange = 4;
                bool inLampRange = false;
                for (int x = spawnPos.x - lampRange; x <= spawnPos.x + lampRange; x++)
                {
                    for (int y = spawnPos.y - lampRange; y <= spawnPos.y + lampRange; y++)
                    {
                        if (tileDefs.lampTile == FGTilemap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            inLampRange = true;
                            break;
                        }
                    }
                }

                // Don't spawn on wood flooring
                bool onWoodFlooring = tileDefs.woodFlooringTile.Equals(BGTilemap.GetTile(spawnPos));

                // Only spawn on tile if tile is empty
                if (FGTilemap.GetTile(spawnPos) == null && !inLampRange && !onWoodFlooring)
                {
                    SpawnBot(spawnPos);

                    spawnTimer = 0;
                    currentSpawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);

                    currentSpeed *= 1.02f; // Speed up bots to make them progressively harder.
                    currentSpeed = Mathf.Min(currentSpeed, 4.75f);

                    break;
                }
            }

        }
        if (!botsMaxedOut)
        {
            spawnTimer += Time.deltaTime;
        }
    }
}
