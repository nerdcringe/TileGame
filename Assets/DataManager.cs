using System.Collections;
using System.Text;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System;

public class DataManager : MonoBehaviour
{
    public const string fileExtension = ".txt";
    public static string saveLocation;
    public string lastSaveName;

    public TileDefs tileDefs;

    // Start is called before the first frame update
    public void Start()
    {
        saveLocation = Application.persistentDataPath + "/Saves/";
        Init();
        print(saveLocation);
    }

    public void Init()
    {
        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }
    }

    // Update is called once per frame
    public void SaveData(string name, string saveData, bool overwrite)
    {
        string fileName = name;

        // If shouldn't overwrite (when new file is created) and saves to already used filename, add number to save name until unique name is used.
        if (!overwrite)
        {
            int saveNum = 1;
            while (File.Exists(saveLocation + fileName + fileExtension))
            {
                saveNum++;
                fileName = name + "_" + saveNum;
            }
        }
        lastSaveName = fileName;
        File.WriteAllText(saveLocation + fileName + fileExtension, saveData);
    }

    public string LoadData(string fileName)
    {
        string file = saveLocation + fileName + fileExtension;
        if (File.Exists(file))
        {
            return File.ReadAllText(file);
        }
        else
        {
            return null;
        }
    }

    // First section of file is seed.
    public float GetSeed(string saveData)
    {
        string seed = saveData.Split(';')[0];
        return float.Parse(seed); // Turn string to int and return.
    }

    public string CreateDataFromTilemap(Tilemap tilemap)
    {
        int w = NoiseGen.width;
        int h = NoiseGen.height;

        StringBuilder sb = new StringBuilder("", w*h);

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int id = 0;
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    id = tileDefs.GetTileFromName(tile.name).id;
                }
                sb.Append(id);

                if (x < w - 1) // Add space between tile id's in same row.
                {
                    sb.Append(" ");
                } else
                {
                    print(x);
                }
            }
            sb.Append("," + Environment.NewLine); // Add comma between rows.
        }
        sb.Append(";"); // Add semicolon after tilemap to separate from other sections.

        return sb.ToString();
    }

    // Set tilemap to save data from file.
    public void FillTilemapFromData(Tilemap tilemap, string saveData, bool inFG)
    {
        // Get the first or second section separated by semicolon, depending on whether tilemap is in bg or fg.
        int dataSectionIndex = 1;
        if (inFG)
        {
            dataSectionIndex = 2;
        }
        string tilemapString = saveData.Split(';')[dataSectionIndex];

        // Separate data string into rows
        string[] rowStrings = tilemapString.Split(',');

        for (int y = 0; y < rowStrings.Length; y++)
        {
            // Get tile id strings in the row;
            string[] tileIDStrings = rowStrings[y].Split(' ');

            for (int x = 0; x < tileIDStrings.Length - 1; x++)
            {
                // Get each tile id in row separated by spaces.
                string tileID = tileIDStrings[x];
                int id = int.Parse(tileID);

                // id 0 is air.
                Tile tile = null;
                if (id != 0)
                {
                    TileType tileType = tileDefs.GetTileFromID(id);
                    if (tileType != null)
                    {
                        tile = tileType.tile;
                    }
                }
                // Convert string to int and put tile of tile id on correct tilemap position.
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    public void SaveWorldToFile(string name, float seed, Tilemap bgTilemap, Tilemap fgTilemap, bool overwrite)
    {
        string dataString = "" + seed + ";" + Environment.NewLine;
        dataString += CreateDataFromTilemap(bgTilemap);
        dataString += CreateDataFromTilemap(fgTilemap);
        SaveData(name, dataString, overwrite);
    }

    public void DeleteFile(string name)
    {
        string file = saveLocation + name + fileExtension;
        if (File.Exists(file))
        {
            print(name + " deleted. (Legit)");
            File.Delete(file);
        }
    }
}
