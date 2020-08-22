using System.Collections;
using System.Text;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class DataManager : MonoBehaviour
{
    const string fileExtension = ".txt";
    static string saveLocation;

    public TileDefs tileDefs;

    // Start is called before the first frame update
    private void Start()
    {  
        saveLocation = Application.dataPath + "/Saves/";
        Init();
    }

    public void Init()
    {
        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        foreach (string file in Directory.GetFiles(saveLocation))
        {
            // Instantiate(saveFab)
        }
    }

    public void detectFiles()
    {
        
    }

    // Update is called once per frame
    public void SaveData(string name, string saveData, bool overwrite)
    {
        string fileName = name;

        // If shouldn't overwrite and saves to already used filename, add number to save name until unique name is used.
        if (!overwrite)
        {
            int saveNum = 1;
            while (File.Exists(saveLocation + fileName + fileExtension))
            {
                saveNum++;
                fileName = name + "_" + saveNum;
            }
        }
        File.WriteAllText(saveLocation + fileName + fileExtension, saveData);
    }

    public string LoadData(string fileName)
    {
        string loadLocation = saveLocation + fileName + fileExtension;
        if (File.Exists(loadLocation))
        {
            return File.ReadAllText(loadLocation);
        }
        else
        {
            return null;
        }
    }

    public string CreateDataFromTilemap(Tilemap tilemap)
    {
        int w = NoiseGen.width;
        int h = NoiseGen.height;

        StringBuilder sb = new StringBuilder("", w*h);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int id = -1;
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    id = tileDefs.GetTileFromName(tile.name).id;
                }
                sb.Append(id + " "); // Add space between tile id's in same row.
            }
            sb.Append(","); // Add comma between rows.
        }
        sb.Append(";"); // Add semicolon after tilemap to separate from other sections.

        return sb.ToString();
    }

    // Set tilemap to save data from file.
    public void FillTilemapFromData(Tilemap tilemap, string saveData, bool inFG)
    {
        // Get the first or second section separated by semicolon, depending on whether tilemap is in bg or fg
        int dataSectionIndex = 0;
        if (inFG)
        {
            dataSectionIndex = 1;
        }
        string tilemapString = saveData.Split(';')[dataSectionIndex];

        // Separate data string into rows
        string[] rowStrings = saveData.Split(',');

        for (int x = 0; x < rowStrings[0].Length; x++)
        {
            for (int y = 0; y < rowStrings.Length; y++)
            {
                string row = rowStrings[y];
                // Get each tile id in row separated by spaces.
                string tileID = row.Split(' ')[0];

                TileType tileType = tileDefs.GetTileFromID(int.Parse(tileID));
                Tile tile = null;
                if (tileType != null)
                {
                    tile = tileType.tile;
                }

                // Convert string to int and put tile of tile id on correct tilemap position.
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
