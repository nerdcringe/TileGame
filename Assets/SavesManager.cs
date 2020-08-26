using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class SavesManager : MonoBehaviour
{
    public DataManager dataManager;

    public Dropdown dropDown;
    public Button selectButton;
    public Button deleteButton;

    bool started = false;

    string GetNameMinusExtension(string s)
    {
        return s.Substring(0, s.Length - DataManager.fileExtension.Length);
    }

    public void Start()
    {
        started = true;
        RefreshSavesList(); 
    }

    void RefreshSavesList()
    {
        dropDown.ClearOptions();
        dropDown.options.Add(new OptionData("NONE"));
        //selectButton.interactable = false;

        DirectoryInfo di = new DirectoryInfo(DataManager.saveLocation);
        FileInfo[] fi = di.GetFiles("*.txt");
        foreach (FileInfo file in fi)
        {
            dropDown.options.Add(new OptionData(GetNameMinusExtension(file.Name)));
        }
        dropDown.Select();
    }

    // Start is called before the first frame update
    public void OnEnable()
    {
        if (started)
        {
            RefreshSavesList();
        }
    }

    // Don't allow pressing select button when selecting default none value (first) in list.
    public void DetermineButtonDisabled()
    {
        bool selecting = dropDown.value > 0;
        selectButton.interactable = selecting;
        deleteButton.interactable = selecting;
    }

    public void DeleteSelectedFile()
    {
        dataManager.DeleteFile(dropDown.captionText.text);
        RefreshSavesList();
    }
}