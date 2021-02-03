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
        DirectoryInfo di = new DirectoryInfo(DataManager.saveLocation);
        FileInfo[] fi = di.GetFiles("*.txt");;

        System.Array.Sort<FileInfo>(fi, Comparer<FileInfo>.Create((f1, f2) =>
            System.DateTime.Compare(f2.LastWriteTime, f1.LastWriteTime)));

        foreach (FileInfo file in fi)
        {
            dropDown.options.Add(new OptionData(GetNameMinusExtension(file.Name)));
        }
        if (fi.Length > 0)
        {
            dropDown.value = 0;
            dropDown.captionText.text = dropDown.options[0].text;
            dropDown.Select();
        }
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
        /*bool selecting = dropDown.value > 0;
        selectButton.interactable = selecting;
        deleteButton.interactable = selecting;*/
    }

    public void DeleteSelectedFile()
    {
        dataManager.DeleteFile(dropDown.captionText.text);
        RefreshSavesList();
    }
}