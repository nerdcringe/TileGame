using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    public DataManager dataManager;
    public SavesManager savesManager;

    public UIControls uiControls;
    public BGTileManager bgTileManager;
    public FGTileManager fgTileManager;

    public InputField saveNameInput;
    public GameObject resumeLastButton;

    public GameObject titleMenu;
    public GameObject savesMenu;
    public GameObject tileEditor;
    public GameObject inGameUI;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject gameOverScreen;

    public string loadedFileName;
    public bool inGame = false;
    public bool paused = false;

    bool generateAlready = false;
    

    // Start is called before the first frame update
    void Start()
    {
        resumeLastButton.SetActive(false);
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
                        PauseMenu();
                    }
                    else
                    {
                        PlayGame();
                    }
                }
                else
                {
                    uiControls.EditTiles();
                }
            }

            if (savesMenu.activeSelf)
            {
                TitleMenu();
            }
            if (settingsMenu.activeSelf)
            {
                dataManager.SaveSettings();
                TitleMenu();
            }
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        inGameUI.SetActive(false);
        tileEditor.SetActive(false);
        inGame = false;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        uiControls.EditTiles();
        pauseMenu.SetActive(false);
        titleMenu.SetActive(false);
        savesMenu.SetActive(false);
        inGameUI.SetActive(true);
        tileEditor.SetActive(true);

        if (!resumeLastButton.activeSelf)
        {
            resumeLastButton.SetActive(true);
        }

        inGame = true;
        paused = false;
    }

    public void PauseMenu()
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
        settingsMenu.SetActive(false);
        paused = false;
    }

    public void SavesMenu()
    {
        titleMenu.SetActive(false);
        savesMenu.SetActive(true);
    }

    public void SettingsMenu()
    {
        titleMenu.SetActive(false);
        settingsMenu.SetActive(true);
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
            dataManager.ResetLoadedGame();
            NoiseGen.seed = Random.Range(0, 99999.99f);
            Generate();
        }
        generateAlready = false;
        saveNameInput.text = "";
        PlayGame();
    }

    public void SelectSave()
    {
        loadedFileName = savesManager.dropDown.captionText.text;
        string dataString = dataManager.ReadFile(loadedFileName);
        dataManager.ResetLoadedGame();
        dataManager.LoadSaveData(dataString);

        generateAlready = false;
        saveNameInput.text = loadedFileName;
        PlayGame();   
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
        dataManager.SaveToFile(name, NoiseGen.seed, bgTileManager.tilemap, fgTileManager.tilemap, overwrite);
        saveNameInput.text = dataManager.lastSaveName;
    }
}