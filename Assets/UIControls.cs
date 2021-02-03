using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public GameObject tileEditor;
    public MenuControls menuControls;
    public GameObject craftingMenu;
    public Crafting crafting;

    public GameObject gameOverScreen;
    public PlayerHealth player;
    public DataManager dataManager;

    public bool gameOver = false;
    public bool craftingOpened = false;

    public void EditTiles()
    {
        craftingOpened = false;
        craftingMenu.SetActive(false);
        tileEditor.SetActive(true);
        gameOverScreen.SetActive(false);
    }
    
    public void Craft()
    {
        craftingOpened = true;
        crafting.Clear();
        tileEditor.SetActive(false);
        craftingMenu.SetActive(true);
    }

    public void Respawn()
    {
        EditTiles();
        dataManager.Respawn();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown("e"))
            {
                craftingOpened = !craftingOpened;
                if (craftingOpened)
                {
                    Craft();
                }
                else
                {
                    EditTiles();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                craftingOpened = false;
                EditTiles();
            }
        }

        if (player.health <= 0 && !gameOver)
        {
            craftingOpened = false;
            craftingMenu.SetActive(false);
            tileEditor.SetActive(false);
            gameOverScreen.SetActive(true);
            gameOver = true;
        }
    }
}
