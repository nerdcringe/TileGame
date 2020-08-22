using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public GameObject tileEditor;
    public MenuControls menuControls;
    public GameObject craftingMenu;
    public Crafting crafting;

    public bool craftingOpened = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void EditTiles()
    {
        craftingOpened = false;
        craftingMenu.SetActive(false);
        tileEditor.SetActive(true);
    }
    
    public void Craft()
    {
        craftingOpened = true;
        crafting.Clear();
        craftingMenu.SetActive(true);

        tileEditor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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
            EditTiles();
        }
    }
}
