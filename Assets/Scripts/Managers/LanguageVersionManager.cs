using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SharedDatastructures;

public class LanguageVersionManager : MonoBehaviour
{

    // LanguageVersionManager is responsible for the Main Menu of the game, including the settings
    // The scripts "GameMode" and "PlayerFeatures" use this to keep the language and version consistent 


    [Header("Language: (True = Danish, False = English)")]
    public bool languagePref = true;
    [Header("GameMode: (True = Battery, False = Interval)")]
    public bool gamePref = true;
    [Header("BCI Pref: (True = Online, False = Simulated)")]
    public bool BCIPref = true;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        languagePref = true; // Danish
        gamePref = true; // Battery
    }

    public void setLanguageDanish()
    {
        // Set the option to the desired setting
        languagePref = true;

        // Highlight the correct button
        GameObject activeButton = GameObject.Find("DKButton");
        GameObject nonActiveButton = GameObject.Find("ENButton");

        activeButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,1);
        nonActiveButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,0);
    }

    public void setLanguageEnglish()
    {
        languagePref = false;

        GameObject activeButton = GameObject.Find("ENButton");
        GameObject nonActiveButton = GameObject.Find("DKButton");

        activeButton.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().color = new(1,1,1,1);
        nonActiveButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,0);
    }

        public void setGamePrefBattery()
    {
        gamePref = true;

        GameObject activeButton = GameObject.Find("BATButton");
        GameObject nonActiveButton = GameObject.Find("INTButton");

        activeButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,1);
        nonActiveButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,0);
    }

    public void setGamePrefInterval()
    {
        gamePref = false;

        GameObject activeButton = GameObject.Find("INTButton");
        GameObject nonActiveButton = GameObject.Find("BATButton");

        activeButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,1);
        nonActiveButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,0);
    }

    public void setBCIPrefOnline()
    {
        BCIPref = true;

        GameObject activeButton = GameObject.Find("BCIButton");
        GameObject nonActiveButton = GameObject.Find("SIMButton");

        activeButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,1);
        nonActiveButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,0);
    }

    public void setBCIPrefSim()
    {
        BCIPref = false;

        GameObject activeButton = GameObject.Find("SIMButton");
        GameObject nonActiveButton = GameObject.Find("BCIButton");

        activeButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,1);
        nonActiveButton.transform.Find("Image").GetComponentInChildren<UnityEngine.UI.Image>().color = new(1,1,1,0);
    }

    public void goToNextScene()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }
}
