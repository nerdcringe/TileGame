using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MenuControls : MonoBehaviour
{
    public DataManager dataManager;

    public UIControls uiControls;
    public Tilemap bgTilemap;
    public Tilemap fgTilemap;

    public GameObject titleMenu;
    public GameObject inGameUI;
    public GameObject pauseMenu;

    public bool inGame = false;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        TitleMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGame)
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
        inGameUI.SetActive(true);
        inGame = true;
    }

    public void PauseGame()
    {
        StopGame();
        titleMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void TitleMenu()
    {
        StopGame();
        titleMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void New()
    {
        NoiseGen.Generate();
        ResumeGame();
    }

    public void Load(string fileName)
    {
        string data = dataManager.LoadData(fileName);
        //NoiseGen.Generate(dataManager.getSeed(fileName))
        dataManager.FillTilemapFromData(bgTilemap, data, false);
        dataManager.FillTilemapFromData(fgTilemap, data, true);

        ResumeGame();
    }
}
