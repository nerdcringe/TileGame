using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MenuControls : MonoBehaviour
{
    public DataManager dataManager;

    public UIControls uiControls;
    public BGTileManager bgTileManager;
    public FGTileManager fgTileManager;

    public GameObject titleMenu;
    public GameObject savesMenu;
    public GameObject inGameUI;
    public GameObject pauseMenu;

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
                print("Yote");
                uiControls.EditTiles();
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
            Generate();
        }
        generateAlready = false;
        ResumeGame();
    }

    public void Load()
    {

        ResumeGame();

        titleMenu.SetActive(false);
        savesMenu.SetActive(true);
    }

    public void LoadSave(string fileName)
    {
        string data = dataManager.LoadData(fileName);
        NoiseGen.Generate(dataManager.GetSeed(fileName));
        dataManager.FillTilemapFromData(bgTileManager.tilemap, data, false);
        dataManager.FillTilemapFromData(fgTileManager.tilemap, data, true);
    }
}
