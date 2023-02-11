using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class LoadDialog : MonoBehaviour
{
    public TMP_Dropdown patternName;

    public Hud hud;

    public void SavePattern()
    {
        ReloadOptions();
    }

    private void OnEnable()
    {
        ReloadOptions();
    }

    void ReloadOptions()
    {
        List<string> options = new List<string>();

        string[] filePaths = Directory.GetFiles(@"patterns/");

        for(int i = 0; i <filePaths.Length; i++)
        {
            string fileName = filePaths[i].Substring(filePaths[i].LastIndexOf('/') + 1);
            string extention = System.IO.Path.GetExtension(fileName);

            fileName = fileName.Substring(0, fileName.Length - extention.Length);

            options.Add(fileName);
        }

        patternName.ClearOptions();
        patternName.AddOptions(options);
    }

    public void QuitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void LoadPattern()
    {
        EventManager.TriggerEvent("LoadPattern");

        hud.isActive = false;
        gameObject.SetActive(false);
    }

}
