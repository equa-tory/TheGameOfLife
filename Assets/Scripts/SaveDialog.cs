using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveDialog : MonoBehaviour
{
    public TMP_InputField patternName;
    public Hud hud;

    public void SavePattern()
    {
        EventManager.TriggerEvent("SavePattern");

        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void QuitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }


}
