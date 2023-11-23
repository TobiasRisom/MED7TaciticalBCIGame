using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Danishify : MonoBehaviour
{
    private GameMode gameMode;
    private TextMeshProUGUI textField;

    [Header("Danish Text Variant:")]
    [TextArea] public string danishText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameMode = gameManager.GetComponent<GameMode>();
        textField = this.GetComponent<TextMeshProUGUI>();

        if(gameMode.danish == true)
        {
            textField.text = danishText;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
