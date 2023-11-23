using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SharedDatastructures;

public class GameMode : MonoBehaviour
{

    public Gamemode gamemode;

    public GameObject player;

    [Header("Change Language (Default = English)")]
    public bool danish = false;
    
    private BciSlider bciSlider;
    private Player playerScript;
    private PlayerFeatures playerFeatures;
    private LanguageVersionManager LVManager;
    private LoggingManager _loggingManager;

    private void Awake()
    {
        _loggingManager = GameObject.Find("LoggingManager").GetComponent<LoggingManager>();
        bciSlider = player.GetComponent<BciSlider>();
        playerScript = player.GetComponent<Player>();
        playerFeatures = player.GetComponent<PlayerFeatures>();
        LVManager = GameObject.Find("Language/VersionManager").GetComponent<LanguageVersionManager>();
        
        bciSlider.gamemode = playerScript.gamemode = playerFeatures.gamemode = gamemode;
        
        
    }

    private void Start()
    {
        logData();
        if(LVManager != null)
        {
            if(LVManager.languagePref == true) // Danish
            {
                danish = true;
            }
            else // English
            {
                danish = false;
            }
        }

    }
    

    private void logData()
    {
        _loggingManager.Log("Game", new Dictionary<string, object>()
        {
            {"Gamemode", gamemode},
            {"Event", "Scene Start"},
        });

    }
}

/*
Tager komponenterne og putter en gamemode variabel ind I det og sætter den variabel til at være tilsvarende gamemode. 
Altså interval/Battery
Log game mode nemt også.


*/
