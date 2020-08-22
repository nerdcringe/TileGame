using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CannonManager : MonoBehaviour
{
    public Dictionary<Vector3Int, float> cannons = new Dictionary<Vector3Int, float>();
    public Tilemap tilemap;
    public TileDefs tileDefs;

    const float detectionDistance = 6;
    const float shootInterval = 4;
    const float speed = 7.5f;

    public Vector3Int pos;
    public GameObject cannonBallFab;


    // Start is called before the first frame update
    void Start()
    {
    }

    GameObject GetClosestBotTo(Vector3Int pos)
    {
        GameObject closestBot = null;
        float closestDistance = 100;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float currentDistance = Vector3.Distance(bot.transform.position, pos);
            if (currentDistance < closestDistance)
            {
                closestBot = bot;
                closestDistance = currentDistance;
            }
        }
        return closestBot;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Vector3Int pos in new List<Vector3Int>(cannons.Keys))
        {
            if (cannons[pos] > shootInterval)
            {
                GameObject bot = GetClosestBotTo(pos);

                if (bot != null && Vector3.Distance(bot.transform.position, pos) < detectionDistance)
                {
                    GameObject cannonBall = GameObject.Instantiate(cannonBallFab);
                    cannonBall.transform.position = pos;

                    // Set velocity of cannonball to face nearest bot at intended speed
                    Vector3 dir = bot.transform.position - pos;
                    dir.Normalize();
                    cannonBall.GetComponent<CannonBall>().vel = dir * speed;

                    cannonBall.transform.position += dir * 1.5f;
                }
                cannons[pos] = 0;

            }
            cannons[pos] += Time.deltaTime;

            // Remove cannon from list if cannon is missing.
            if (tilemap.GetTile(pos) != tileDefs.cannonTile)
            {
                cannons.Remove(pos);
            }
        }
    }
}
