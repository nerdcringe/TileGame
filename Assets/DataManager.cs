using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public const string fileExtension = ".txt";
    public static string saveLocation;
    public const string settingsFileName = "settings";
    public static string settingsLocation;

    public TileDefs tileDefs;
    public BGTileManager bgTileManager;
    public FGTileManager fgTileManager;

    public PlayerMovement player;
    public PlayerHealth health;
    public Inventory inv;
    public CookingManager cookingManager;
    public CannonManager cannonManager;
    public BotSpawning botSpawning;
    public FishSpawning fishSpawning;

    public AudioManager audioManager;
    public Slider volumeSlider;

    public string lastSaveName;

    // Start is called before the first frame update
    public void Start()
    {
        saveLocation = Application.persistentDataPath + "/Saves/";
        settingsLocation = Application.persistentDataPath + "/";
        Init();
        LoadSettings();
        //print(saveLocation);
    }

    public void Init()
    {
        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }
    }

    public void WriteFile(string location, string name, string saveData, bool overwrite)
    {
        string fileName = name;

        // If shouldn't overwrite (when new file is created) and saves to already used filename, add number to save name until unique name is used.
        if (!overwrite)
        {
            int saveNum = 1;
            while (File.Exists(location + fileName + fileExtension))
            {
                saveNum++;
                fileName = name + "_" + saveNum;
            }
        }
        lastSaveName = fileName;
        File.WriteAllText(location + fileName + fileExtension, saveData);
    }

    public string ReadFile(string fileName)
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

    public void DeleteFile(string name)
    {
        string file = saveLocation + name + fileExtension;
        if (File.Exists(file))
        {
            print(name + " deleted. (Legit)");
            File.Delete(file);
        }
    }


    public string CreateDataFromTilemap(Tilemap tilemap)
    {
        int w = NoiseGen.width;
        int h = NoiseGen.height;

        StringBuilder sb = new StringBuilder("", w * h);

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
                }
            }
            sb.Append(","); // Add comma between rows.
        }

        return sb.ToString();
    }

    // Set tilemap to save data from file.
    public void FillTilemapFromData(Tilemap tilemap, string tilemapString)
    {
        tilemap.ClearAllTiles();
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


    public void SaveToFile(string name, float seed, Tilemap bgTilemap, Tilemap fgTilemap, bool overwrite)
    {
        string dataString;
        SaveData saveData = new SaveData();

        saveData.playerX = player.transform.position.x;
        saveData.playerY = player.transform.position.y;
        saveData.health = health.health;


        foreach (Vector3Int doorPos in player.openedDoorPos)
        {
            saveData.openDoorXs.Add(doorPos.x);
            saveData.openDoorYs.Add(doorPos.y);
        }

        foreach (TileType tileType in inv.items.Keys)
        {
            saveData.inventoryItemIDs.Add(tileType.id);
            saveData.inventoryItemAmounts.Add(inv.items[tileType]);
        }

        // Keep track of meat positions and cooking durations.
        foreach (Vector3Int pos in cookingManager.meats.Keys)
        {
            saveData.meatXs.Add(pos.x);
            saveData.meatYs.Add(pos.y);
            saveData.meatCookingDurations.Add(cookingManager.meats[pos]);
        }
        foreach (Vector3Int pos in cannonManager.cannons.Keys)
        {
            saveData.cannonXs.Add(pos.x);
            saveData.cannonYs.Add(pos.y);
            saveData.cannonLoadTimes.Add(cannonManager.cannons[pos]);
        }

        saveData.botSpeed = botSpawning.currentSpeed;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            saveData.botXs.Add(bot.transform.position.x);
            saveData.botYs.Add(bot.transform.position.y);
        }
        foreach (GameObject fish in GameObject.FindGameObjectsWithTag("Fish"))
        {
            saveData.fishXs.Add(fish.transform.position.x);
            saveData.fishYs.Add(fish.transform.position.y);
        }

        saveData.seed = NoiseGen.seed;
        saveData.bgTilemapData = CreateDataFromTilemap(bgTilemap);
        saveData.fgTilemapData = CreateDataFromTilemap(fgTilemap);

        dataString = JsonUtility.ToJson(saveData);
        Init();
        WriteFile(saveLocation, name, dataString, overwrite);
    }


    public void Respawn()
    {
        player.speed = PlayerMovement.initialSpeed;
        player.rb.velocity = Vector3.zero;
        player.transform.position = new Vector3(NoiseGen.width / 2, NoiseGen.height / 2);
        player.targetPos = new Vector3Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y), 0);
        player.transform.position = player.targetPos;
        health.health = 4;

        inv.items.Clear();
        inv.currentTileIndex = 0;
        inv.currentTileType = null;

        botSpawning.currentSpeed = BotSpawning.initialSpeed;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject.Destroy(bot);
        }
        foreach (GameObject fish in GameObject.FindGameObjectsWithTag("Fish"))
        {
            GameObject.Destroy(fish);
        }
    }

    public void ResetLoadedGame()
    {
        Respawn();
        cookingManager.meats.Clear();
        cannonManager.cannons.Clear();
    }


    public void LoadSaveData(string jsonString)
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonString);

        player.transform.position = new Vector3(saveData.playerX, saveData.playerY, 0);
        health.health = saveData.health;

        for(int i = 0; i < saveData.inventoryItemIDs.Count; i++)
        {
            TileType tileType = tileDefs.GetTileFromID(saveData.inventoryItemIDs[i]);
            inv.AddItem(tileType, saveData.inventoryItemAmounts[i]);
        }

        for (int i = 0; i < saveData.openDoorXs.Count; i++)
        {
            player.openedDoorPos.Add(new Vector3Int(saveData.openDoorXs[i], saveData.openDoorYs[i], 0));
        }
        for (int i = 0; i < saveData.meatXs.Count; i++)
        {
            cookingManager.meats.Add(new Vector3Int(saveData.meatXs[i], saveData.meatYs[i], 0), saveData.meatCookingDurations[i]);
        }
        for (int i = 0; i < saveData.cannonXs.Count; i++)
        {
            cannonManager.cannons.Add(new Vector3Int(saveData.cannonXs[i], saveData.cannonYs[i], 0), saveData.cannonLoadTimes[i]);
        }

        botSpawning.currentSpeed = saveData.botSpeed;
        for (int i = 0; i < saveData.botXs.Count; i++)
        {
            botSpawning.SpawnBot(new Vector3(saveData.botXs[i], saveData.botYs[i], 0));
        }
        for (int i = 0; i < saveData.fishXs.Count; i++)
        {
            fishSpawning.SpawnFish(new Vector3(saveData.fishXs[i], saveData.fishYs[i], 0));
        }

        NoiseGen.seed = saveData.seed;
        NoiseGen.Generate();
        FillTilemapFromData(bgTileManager.tilemap, saveData.bgTilemapData);
        FillTilemapFromData(fgTileManager.tilemap, saveData.fgTilemapData);
    }


    public void SaveSettings()
    {
        SettingsData settingsData = new SettingsData();
        settingsData.volumeAmount = audioManager.volume;

        string settingsString = JsonUtility.ToJson(settingsData);
        WriteFile(settingsLocation, settingsFileName, settingsString, true);
    }

    public void LoadSettings()
    {
        string settingsString = ReadFile(settingsLocation + settingsFileName + fileExtension);

        if (settingsString != null)
        {
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(settingsString);
            volumeSlider.value = settingsData.volumeAmount;
            audioManager.SetVolume();
        }
    }


    [Serializable]
    public class SaveData
    {
        public float playerX;
        public float playerY;
        public int health;
        public List<int> inventoryItemIDs = new List<int>();
        public List<int> inventoryItemAmounts = new List<int>();

        public List<int> openDoorXs = new List<int>();
        public List<int> openDoorYs = new List<int>();

        public List<int> meatXs = new List<int>();
        public List<int> meatYs = new List<int>();
        public List<float> meatCookingDurations = new List<float>();

        public List<int> cannonXs = new List<int>();
        public List<int> cannonYs = new List<int>();
        public List<float> cannonLoadTimes = new List<float>();

        public float botSpeed = 3.4f;
        public List<float> botXs = new List<float>();
        public List<float> botYs = new List<float>();

        public List<float> fishXs = new List<float>();
        public List<float> fishYs = new List<float>();

        public float seed;
        public string bgTilemapData;
        public string fgTilemapData;

    }

    [Serializable]
    public class SettingsData
    {
        public float volumeAmount;
    }
}