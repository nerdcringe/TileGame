using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    public DataManager dataManager;
    public SavesManager savesManager;

    public UIControls uiControls;
    public BGTileManager bgTileManager;
    public FGTileManager fgTileManager;

    public InputField saveNameInput;

    public GameObject titleMenu;
    public GameObject savesMenu;
    public GameObject inGameUI;
    public GameObject pauseMenu;

    public string loadedFileName;
    public bool inGame = false;
    public bool paused = false;

    bool generateAlready = false;
    

    // Start is called before the first frame update
    void Start()
    {
        TitleMenu();
        Generate();
        generateAlready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGameUI.activeSelf || pauseMenu.activeSelf)
            {
                if (!uiControls.craftingOpened)
                {
                    paused = !paused;

                    if (paused)
                    {
                        PauseGame();
                    }
                    else
                    {
                        ResumeGame();
                    }
                }
                else
                {
                    uiControls.EditTiles();
                }
            }
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        inGameUI.SetActive(false);
        inGame = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        uiControls.EditTiles();
        pauseMenu.SetActive(false);
        titleMenu.SetActive(false);
        savesMenu.SetActive(false);
        inGameUI.SetActive(true);
        inGame = true;
        paused = false;
    }

    public void PauseGame()
    {
        StopGame();
        titleMenu.SetActive(false);
        pauseMenu.SetActive(true);
        paused = true;
    }

    public void TitleMenu()
    {
        StopGame();
        titleMenu.SetActive(true);
        pauseMenu.SetActive(false);
        savesMenu.SetActive(false);
        paused = false;
    }

    public void Generate()
    {
        NoiseGen.Generate();
        fgTileManager.tilemap.ClearAllTiles();
        fgTileManager.Generate();
        bgTileManager.Generate();
    }

    public void New()
    {
        if (!generateAlready)
        {
            NoiseGen.seed = Random.Range(0, 99999.99f);
            Generate();
        }
        generateAlready = false;
        ResumeGame();
        saveNameInput.text = "";
    }

    public void Load()
    {
        titleMenu.SetActive(false);
        savesMenu.SetActive(true);
    }

    public void SelectSave()
    {
        loadedFileName = savesManager.dropDown.captionText.text;
        print(loadedFileName);
        string data = dataManager.LoadData(loadedFileName);
        NoiseGen.seed = dataManager.GetSeed(data);
        NoiseGen.Generate();

        dataManager.FillTilemapFromData(bgTileManager.tilemap, data, false);
        dataManager.FillTilemapFromData(fgTileManager.tilemap, data, true);
        saveNameInput.text = loadedFileName;
    }

    public void SaveCurrentGame()
    {
        string name = saveNameInput.text;
        bool overwrite = true;

        if (name == "")
        {
            name = "save";
            overwrite = false;
        }
        dataManager.SaveWorldToFile(name, NoiseGen.seed, bgTileManager.tilemap, fgTileManager.tilemap, overwrite);
        saveNameInput.text = dataManager.lastSaveName;
    }
}