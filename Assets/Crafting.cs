using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public const int width = 2;
    public const int height = 2;
    public const int size = width * height;

    public Inventory inv;
    public TileDefs tileDefs;
    public RecipeDefs recipeDefs;
    public AudioManager audioManager;

    public GameObject craftingMenu;
    public Image outputIcon;
    public BoxCollider2D outputCollider;
    public Text outputCounter;

    public List<TileType> gridTileTypes = new List<TileType>();
    public Tilemap grid;

    Recipe currentRecipe;


    public int GetAmountOnGrid(TileType tileType)
    {
        int num = 0;
        foreach (TileType tile in gridTileTypes)
        {
            if (tileType != null)
            {
                if (tileType.name == tile.name)
                {
                    num++;
                }
            }
        }
        return num;
    }

    public void Clear()
    {
        gridTileTypes.Clear();
        grid.ClearAllTiles();
        currentRecipe = null;
        outputIcon.enabled = false;
        outputCounter.text = "";
        outputIcon.sprite = null;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        gridPos.z = 0;
        TileBase preexistingTile = grid.GetTile(gridPos);
        TileType tileType = inv.currentTileType;

        if (gridPos.x <= 0 && gridPos.x > -width && gridPos.y <= 0 && gridPos.y > -height)
        {
            if (Input.GetMouseButton(1))
            {
                if (tileType != null)
                {
                    if (GetAmountOnGrid(tileType) < inv.items[tileType] && tileType != null && preexistingTile == null)
                    {
                        grid.SetTile(gridPos, tileType.tile);
                        audioManager.PlaySound(tileType.sound, Camera.main.transform.position);
                        gridTileTypes.Add(tileType);
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (preexistingTile != null)
                {
                    gridTileTypes.Remove(tileDefs.GetTileFromName(preexistingTile.name));
                    audioManager.PlaySound(tileType.sound, Camera.main.transform.position);
                    grid.SetTile(gridPos, null);
                }
            }
        }

        bool anyArrangementMatches = false;
        // Check which recipes match arrangement in table.
        foreach (Recipe recipe in recipeDefs.recipes)
        {
            for (int i = 0; i < recipe.inputs.GetLength(0); i++)
            {
                bool allTilesMatch = true;

                TileType tl = recipe.inputs[i, 0];
                TileType tr = recipe.inputs[i, 1];
                TileType bl = recipe.inputs[i, 2];
                TileType br = recipe.inputs[i, 3];

                TileBase tlTile = grid.GetTile(new Vector3Int(-1, 0, 0));
                if (tl != null)
                {
                    if (tl.tile != tlTile)
                    {
                        allTilesMatch = false;
                    }
                }
                else
                {
                    if (tlTile != null)
                    {
                        allTilesMatch = false;
                    }
                }
                
                TileBase trTile = grid.GetTile(new Vector3Int(0, 0, 0));
                if (tr != null)
                {
                    if (tr.tile != trTile)
                    {
                        allTilesMatch = false;
                    }
                }
                else
                {
                    if (trTile != null)
                    {
                        allTilesMatch = false;
                    }
                }

                TileBase blTile = grid.GetTile(new Vector3Int(-1, -1, 0));
                if (bl != null)
                {
                    if (bl.tile != blTile)
                    {
                        allTilesMatch = false;
                    }
                }
                else
                {
                    if (blTile != null)
                    {
                        allTilesMatch = false;
                    }
                }

                TileBase brTile = grid.GetTile(new Vector3Int(0, -1, 0));
                if (br != null)
                {
                    if (br.tile != brTile)
                     {
                        allTilesMatch = false;
                    }
                }
                else
                {
                    if (brTile != null)
                    {
                        allTilesMatch = false;
                    }
                }

                if (allTilesMatch)
                {
                    currentRecipe = recipe;
                    anyArrangementMatches = true;
                }
            }
        }
        
        if (!anyArrangementMatches)
        {
            currentRecipe = null;
        }

        if (currentRecipe != null)
        {
            if (outputIcon.sprite != currentRecipe.output.sprite)
            {
                outputIcon.sprite = currentRecipe.output.sprite;
            }
            if (!outputIcon.enabled)
            {
                outputIcon.enabled = true;
            }
            outputCounter.text = "" + currentRecipe.outputAmount;
            
            // Collect newly crafted item.
            if (Input.GetMouseButtonDown(0) && outputCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                inv.AddItem(currentRecipe.output, currentRecipe.outputAmount);
                audioManager.PlaySound(currentRecipe.output.sound, Camera.main.transform.position);

                for (int i = 0; i < 4; i++)
                {
                    if (i < gridTileTypes.Count)
                    {
                        inv.RemoveItem(gridTileTypes[i], 1);
                    }
                }
                outputIcon.enabled = false;
                Clear();
            }
        }
        else
        {
            if (outputIcon.enabled)
            {
                outputIcon.enabled = false;
            }
            outputCounter.text = "";
        }
    }
}
