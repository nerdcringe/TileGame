using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public TileDefs tileDefs;

    // Sorts items by ID in inventory.
    public static Comparer<TileType> itemSorter = Comparer<TileType>.Create((tile1, tile2) => tile1.id.CompareTo(tile2.id));
    public SortedList<TileType, int> items = new SortedList<TileType, int>(itemSorter); // Item type and amounts

    public TileType currentTileType;
    public int currentTileIndex = 0;

    public Image tileIcon;
    public Text tileCounter;

    public Crafting crafting;

    public bool HasItem(TileType tileType, int minAmount)
    {
        if (tileType != null)
        {
            if (items.ContainsKey(tileType))
            {
                return items[tileType] >= minAmount;
            }
        }
        return false;
    }

    public void AddItem(TileType tileType, int amount)
    {
        if (HasItem(tileType, 0))
        {
            items[tileType] = items[tileType] + amount;
        }
        else
        {
            items.Add(tileType, amount);

            if (currentTileType != null)
            {
                if (tileType.id < currentTileType.id)
                {
                    currentTileIndex++;
                }
            }
        }
    }


    public void RemoveItem(TileType tileType, int amount)
    {
        if (tileType != null)
        {
            if (HasItem(tileType, amount))
            {
                items[tileType] = items[tileType] - amount;
            }

            if (items[tileType] <= 0)
            {
                items.Remove(tileType);
                currentTileIndex = Mathf.Clamp(currentTileIndex, 0, items.Count - 1);

                if (items.Count > 0)
                {
                    currentTileType = items.Keys[currentTileIndex];
                }
                else
                {
                    currentTileType = null;
                    currentTileIndex = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (items.Count > 0)
        {
            if (Input.mouseScrollDelta.y >= 1)
            {
                currentTileIndex++;
            }
            if (Input.mouseScrollDelta.y <= -1)
            {
                currentTileIndex--;
            }
            if (currentTileIndex > items.Count - 1)
            {
                currentTileIndex = 0;
            }
            if (currentTileIndex < 0)
            {
                currentTileIndex = items.Count - 1;
            }

            currentTileType = items.Keys[currentTileIndex];
        }
        else
        {
            currentTileIndex = 0;
        }

        if (currentTileType != null)
        {
            if (tileIcon.sprite != currentTileType.sprite)
            {
                tileIcon.sprite = currentTileType.sprite;
            }
            if (!tileIcon.enabled)
            {
                tileIcon.enabled = true;
            }

            int num = items[currentTileType] - crafting.GetAmountOnGrid(currentTileType);
            tileCounter.text = "" + num;
        }
        else
        {
            if (tileIcon.enabled)
            {
                tileIcon.enabled = false;
            }
            tileCounter.text = "";
        }
    }
}
