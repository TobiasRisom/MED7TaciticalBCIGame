using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedDatastructures;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    [Header("Tutorial elements")]
    public int currentArea = 0;
    public int currentEnemy = 0;
    private static readonly List<int> enemyAreas = new List<int>() {2, 4, 5};
    private EnemyHealth[] enemies;
    public GameObject[] spawnPoints;
    private TMPro.TextMeshProUGUI tutorialUI; // Text component
    [TextArea] public string[] tutorialTexts;
    [TextArea] public string[] tutorialTextsDK;

    [Header("Gameplay manipulation")]
    private GameObject player;
    private TurnManager turnManager;
    private Image[] chargeButton;
    private BciSlider bciSlider;
    private GameMode gameMode;
    private bool checkComplete = false;

    private ConfirmBtn confirmBtn;
    public GameObject firstTile;
    public GameObject firstGoblin;
    public Tile tileScript;
    public Highlight goblinHiLi;
    public GameObject mousePointer;
    private Animator mouseAnim;
    private Animator playerAnim;
    private LoggingManager _loggingManager;
    private Transform cameraPivot;
    private AmmoSpawn ammoSpawn;

    void Awake() {
        tutorialUI = GameObject.Find("TutorialText").GetComponent<TMPro.TextMeshProUGUI>();
        GameObject gameManager = GameObject.Find("GameManager");
        gameMode = gameManager.GetComponent<GameMode>();
        turnManager = gameManager.GetComponent<TurnManager>();
        player = GameObject.Find("Player");
        playerAnim = player.GetComponent<Animator>();
        enemies = GameObject.Find("Enemies").GetComponentsInChildren<EnemyHealth>();
        bciSlider = player.GetComponent<BciSlider>();
        tileScript = firstTile.GetComponent<Tile>();
        goblinHiLi = firstGoblin.GetComponent<Highlight>();
        confirmBtn = GameObject.Find("ConfirmBtn").GetComponent<ConfirmBtn>();
        cameraPivot = GameObject.Find("Pivot").transform;
        ammoSpawn = GameObject.Find("Ammo").GetComponent<AmmoSpawn>();

        mouseAnim = mousePointer.GetComponent<Animator>();
    }


    void Start()
    {
        _loggingManager = GameObject.Find("LoggingManager").GetComponent<LoggingManager>();
        bciSlider.showable = false;
        Invoke("delayStart", 1f);
        UpdateArea(); // Update area to init
    }

    void delayStart()
    {
        bciSlider.ShowChargeButton(false);
        PlayerFeatures res = player.GetComponent<PlayerFeatures>();
        if (gameMode.gamemode == Gamemode.Battery)
        {
            //res.mana = 1;
            //res.maxMana = 5;
           // ammoSpawn.SpawnAmmo();
        }
    }
    /*
     This method is invoked after a delay of one second from the start of the tutorial. 
     It prepares the player and the game environment for the tutorial. 
     It sets the player's mana and maxMana based on the game mode. 
     For the battery game mode, it sets the mana to 2 and the maxMana to 10.
    */

    // Update is called once per frame
    void Update() {
        GoToLevel1();
        CheckTutorialComplete(); // Check if last area is complete
        
        CheckMovingArea1(); // Area 1, check player movement (turn end)
        CheckManaArea3(); // Area 3, check mana regain
        CheckEnemyDeath(); // Other areas, check enemy alive
        AnimationPlayer();
    }
    //It contains the main logic for checking the tutorial progress and triggering events accordingly.

    void CheckMovingArea1() {
        if (currentArea != 1) return; // Guard area index
        if (turnManager.playerTurn) return; // Guard player turn
        StartCoroutine(DelayUpdateArea(1f)); // Update area if players turn is over
    }
    /*
    his method checks the completion condition for area 1 (movement tutorial). 
    If it's area 1 and it's not the player's turn, it starts a coroutine 
    (DelayUpdateArea()) to delay the update of the tutorial area.
    */

    void AnimationPlayer()
    {
        if(currentArea == 1 || currentArea == 2)
        {
            if(playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {

                if(tileScript.targetTile == false && goblinHiLi.selected == false)
                {
                    mouseAnim.Play("Tutorial Mouse");
                }
                else
                {
                    mouseAnim.Play("Tutorial Mouse 2");
                }
            }
        
            else
            {
                mouseAnim.Play("Tutorial Mouse Idle");
            }
        }

        else if(currentArea == 3 && bciSlider.ChargeButton.enabled == true)
        {
            mouseAnim.Play("Tutorial Mouse 3");
        }

        else
        {
            mouseAnim.Play("Tutorial Mouse Idle");
        }
        
        
    }

    void CameraUpdate()
    {
        if(currentArea == 3)
        {
            cameraPivot.position = new Vector3(0,0,0);
        }
    }

    void CheckEnemyDeath()
    {
        if (!enemyAreas.Contains(currentArea)) return; // Guard non enemy areas
        if (currentEnemy >= enemies.Length) return; // Guard enemy index
        EnemyHealth enemyHealth = enemies[currentEnemy]; // Select current enemy
        if (enemyHealth.alive) return; // Continue when current selected enemy is dead
        currentEnemy++; // Increment currently selected enemy
        StartCoroutine(DelayUpdateArea(1.75f)); // Update area when selected enemy dies
    }
    /*
    This method checks the completion condition for the tutorial areas with enemies. 
    If the current area is in the list of enemy areas, it checks if the current enemy is dead. 
    If so, it increments the currentEnemy index and starts a coroutine 
    */

   void CheckManaArea3() {
       if (currentArea != 3) return; // Guard not area 3

        // Check if game mode is battery
        if (gameMode.gamemode != Gamemode.Battery) { // Guard other than battery
            UpdateArea(); // Skip this area
            return; // Abort method
        }
        float mana = player.GetComponent<PlayerFeatures>().mana; // Players mana
        float maxMana = player.GetComponent<PlayerFeatures>().maxMana;
        if (mana < maxMana) {
            return;
            } 
        if (gameMode.gamemode == Gamemode.Interval) { UpdateArea(); // when player has gain mana
        } else { StartCoroutine(DelayUpdateArea(1f)); }
    }
    /*
    his method checks the completion condition for area 3 (mana regain tutorial). 
    If it's area 3 and the game mode is battery, it checks if the player's mana is greater than or equal to 4.
    If it's not enough mana, the method returns. 
    If the game mode is not battery, it updates the tutorial area immediately. 
    */

    // Update spawn point and tutorial text
    void UpdateArea() {
        // print("Updating Area!");
        Spawn(); // Spawn player at current spawn location
        UpdateUI(); // Update tutorial UI to current text
        currentArea++; // Increment area value

        if (currentArea == 3 && gameMode.gamemode == Gamemode.Battery)
        {
            bciSlider.showable = true;
            bciSlider.ShowChargeButton(true); // Activate charge button
        }
    }

    /*
    updates tutorial area by doing 3 things.
    spawn, updating UI text, increment current area, if current are is 3, and mode is battery is enables
    bci slider and sows charge button.
    */

    IEnumerator DelayUpdateArea(float sec)
    {
        if (checkComplete) yield break;
        Player playerComp = player.GetComponent<Player>();
        playerComp.moveAllowed = false;
        checkComplete = true;
        yield return new WaitForSeconds(sec);
        //print("Delay!");
        UpdateArea();
        checkComplete = false;
        playerComp.moveAllowed = true;
    }
    /*
    This coroutine delays the update of the tutorial area to allow for certain actions to complete. 
    It temporarily disables player movement, sets checkComplete to true to prevent duplicate updates, waits for the
     specified delay time, updates the area, sets checkComplete back to false, and enables player movement again.
    */

    void UpdateUI() {
        if (currentArea >= tutorialTexts.Length) return; // Guard index error
        if(gameMode.danish == false)
        {
            tutorialUI.text = tutorialTexts[currentArea]; // Update text with tutorial text
        }
        else
        {
           tutorialUI.text = tutorialTextsDK[currentArea]; 
        }
    }

    void Spawn() {
        if (currentArea >= spawnPoints.Length) return; // Guard index error
        GameObject spawnPoint = spawnPoints[currentArea]; // Current spawn point
        player.transform.position = spawnPoint.transform.position; // Update position to spawn point
    }

    void CheckTutorialComplete() {
        if (currentArea < 6) return; // Guard index less than end
        if (checkComplete) return;
        checkComplete = true;
        Debug.Log("Tutorial done!");
        Invoke("GoToNextScene", 2f);
    }
    /*
    his method checks if the tutorial is complete by verifying if the currentArea is greater than or equal to 6. 
    If so, it sets checkComplete to true to prevent duplicate calls and invokes the GoToNextScene() method after a delay of 2 seconds.
    */

    void GoToNextScene()
    {
        _loggingManager.Log("Game", "Event", "End Tutorial");
        SceneManager.LoadScene("Level 2"); // Launch the new scene
    }

    void GoToLevel1() {
        if (!Input.GetKeyUp(KeyCode.F1)) return;
        GoToNextScene();
    }
    //Force to go to lvl it when pressing f1.
}
