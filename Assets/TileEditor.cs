using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileEditor : MonoBehaviour
{
    public Inventory inventory;
    public Tilemap tilemap;
    public TileDefs tileDefs;

    public BGTileManager BGTileManager;
    public FGTileManager FGTileManager;

    public CannonManager cannonManager;
    public GameObject cannonBallFab;
    public CookingManager cooking;

    public Vector3Int tilePos;
    public Vector3Int lastTilePos;
    public Transform player;

    public GameObject tileHighlight;
    SpriteRenderer highlightSR;
    Sprite highlightSprite;
    public Sprite gatherStage0;
    public Sprite gatherStage1;
    public Sprite gatherStage2;
    public Sprite gatherStage3;
    public Sprite gatherStage4;

    public AudioManager audioManager;

    public float gatherTime = 0;

    TileType holdItem;

    // Start is called before the first frame update
    void Start()
    {
        tilePos = new Vector3Int(0, 0, 0);
        lastTilePos = tilePos;
        highlightSR = tileHighlight.GetComponent<SpriteRenderer>();
        highlightSprite = highlightSR.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tilePos = new Vector3Int(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y), 0);
        tileHighlight.transform.position = tilePos;

        TileBase tile = tilemap.GetTile(tilePos);
        TileType selectedTileType = inventory.currentTileType;

        // Make sure when stack of tiles runs out, mouse must be clicked again to use next stack.
        if (Input.GetMouseButtonDown(1))
        {
            holdItem = selectedTileType;
        }

        if (tilePos.x >= 0 && tilePos.x < NoiseGen.width && tilePos.y >= 0 && tilePos.y < NoiseGen.height)
        {
            bool canPlaceWood = !tileDefs.woodFlooringTile.Equals(BGTileManager.tilemap.GetTile(tilePos));

            if (inventory.HasItem(selectedTileType, 1) && !(tilePos.x == Mathf.RoundToInt(player.position.x) && tilePos.y == Mathf.RoundToInt(player.position.y)))
            {
                bool canWaterTree = tileDefs.acornTile.Equals(tilemap.GetTile(tilePos)) && tileDefs.water.Equals(selectedTileType);

                if (Input.GetMouseButton(1) && !Input.GetMouseButton(0) && (tile == null || canWaterTree) && holdItem == selectedTileType &&
                    (!tileDefs.woodFlooring.Equals(selectedTileType) || canPlaceWood))
                {
                    if (canWaterTree)
                    {
                        FGTileManager.Tree(tilePos.x, tilePos.y);
                        audioManager.PlaySound(audioManager.pop, tilePos);
                    }
                    else if (canPlaceWood && tileDefs.woodFlooring.Equals(selectedTileType))
                    {
                        audioManager.PlaySound(selectedTileType.sound, tilePos);
                        BGTileManager.tilemap.SetTile(tilePos, tileDefs.woodFlooringTile);
                    }
                    else
                    {
                        audioManager.PlaySound(selectedTileType.sound, tilePos);
                        tilemap.SetTile(tilePos, selectedTileType.tile);
                    }

                    // Add cannon manager for tile position if cannon is placed
                    if (tileDefs.cannon.Equals(selectedTileType))
                    {
                        cannonManager.cannons.Add(tilePos, 0);
                    }

                    // If raw meat is placed next to campfire, start cooking meat.
                    if (tileDefs.rawMeat.Equals(selectedTileType))
                    {
                        bool nextToCampfire = false;
                        for (int x = tilePos.x - 1; x <= tilePos.x + 1; x++)
                        {
                            for (int y = tilePos.y - 1; y <= tilePos.y + 1; y++)
                            {
                                if (tileDefs.campfireTile.Equals(tilemap.GetTile(new Vector3Int(x, y, 0))))
                                {
                                    nextToCampfire = true;
                                }
                            }
                        }
                        if (nextToCampfire)
                        {
                            cooking.meats.Add(tilePos, 0);
                        }
                    }

                    // If campfire is placed next to any preexisting meat, start cooking all meat around it.
                    if (tileDefs.campfire.Equals(selectedTileType))
                    {
                        for (int x = tilePos.x - 1; x <= tilePos.x + 1; x++)
                        {
                            for (int y = tilePos.y - 1; y <= tilePos.y + 1; y++)
                            {
                                Vector3Int meatPos = new Vector3Int(x, y, 0);
                                if (tileDefs.rawMeatTile.Equals(tilemap.GetTile(meatPos)))
                                {
                                    cooking.meats.Add(meatPos, 0);
                                }
                            }
                        }
                    }

                    inventory.RemoveItem(selectedTileType, 1);
                }
            }

            // Break wood flooring in background and replace it with original bg tile if foreground tile is empty (null).
            bool canBreakWoodFlooring = tilemap.GetTile(tilePos) == null && tileDefs.woodFlooringTile.Equals(BGTileManager.tilemap.GetTile(tilePos));

            if (Input.GetMouseButton(0) && (tile != null || canBreakWoodFlooring))
            {
                if (canBreakWoodFlooring)
                {
                    tile = tileDefs.woodFlooringTile;
                }
                TileType tileType = tileDefs.GetTileFromName(tile.name);
                gatherTime += Time.deltaTime;

                // Set proper gather sprite for gather stage.
                if (gatherTime < tileType.gatherTime * 0.25)
                {
                    highlightSR.sprite = gatherStage1;
                }
                else if (gatherTime < tileType.gatherTime * 0.5)
                {
                    highlightSR.sprite = gatherStage2;
                }
                else if (gatherTime < tileType.gatherTime * 0.75)
                {
                    highlightSR.sprite = gatherStage3;
                }
                else
                {
                    highlightSR.sprite = gatherStage4;
                }

                // If gather time is over the tiletype's gather time, then break the tile.
                if (gatherTime > tileDefs.GetTileFromName(tile.name).gatherTime)
                {
                    audioManager.PlaySound(tileType.sound, tilePos);
                    tilemap.SetTile(tilePos, null);

                    if (canBreakWoodFlooring)
                    {
                        tileType = tileDefs.woodFlooring;
                        BGTileManager.tilemap.SetTile(tilePos, BGTileManager.GetTileAtValue(NoiseGen.noisemap[tilePos.x, tilePos.y]));
                    }

                    if (tileDefs.leaf.Equals(tileType))
                    {
                        if (Random.Range(0, 8) == 0)
                        {
                            tileType = tileDefs.acorn;
                        }
                    }

                    if (tileType == tileDefs.openDoor)
                    {
                        tileType = tileDefs.door;
                    }

                    inventory.AddItem(tileType, 1);
                    gatherTime = 0;
                }
            }
            else
            {
                gatherTime = 0;
            }

            if (!highlightSR.enabled)
            {
                highlightSR.enabled = true;
            }
        }
        else
        {
            if (highlightSR.enabled)
            {
                highlightSR.enabled = false;
            }
        }

        if (lastTilePos != tilePos)
        {
            gatherTime = 0;
        }

        if (gatherTime == 0)
        {
            highlightSR.sprite = highlightSprite;
        }

        lastTilePos = tilePos;
    }
}
