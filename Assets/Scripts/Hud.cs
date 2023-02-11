using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public SaveDialog saveDialog;
    public LoadDialog loadDialog;

    public bool isActive = false;

    void Start()
    {
        saveDialog.gameObject.SetActive(false);
        loadDialog.gameObject.SetActive(false);
    }

    public void ShowSaveDialog()
    {
        saveDialog.gameObject.SetActive(true);
        isActive = true;
    }

    public void ShowLoadDialog()
    {
        loadDialog.gameObject.SetActive(true);
        isActive = true;
    }

}