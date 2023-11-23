using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBtn : MonoBehaviour {
    public List<Sprite> sprites; // Changeable sprites
    private Dictionary<string, Sprite> confirmBtnSprites; // Name : sprite dict
    private Image image; // Image component
    private TMPro.TextMeshProUGUI text; // Text component
    public Button btn;
    private BoxCollider confirmCol;
    private GameMode gameMode;

    void Awake() {
        btn = GetComponent<Button>();
        image = GameObject.Find("ConfirmSprite").GetComponent<Image>(); // Button image component
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>(); // Button image component
        confirmCol = GetComponent<BoxCollider>();
        GameObject gameManager = GameObject.Find("GameManager");
        gameMode = gameManager.GetComponent<GameMode>();

        // Update
        DisableImage();
        FormatSprites(); // Format sprites from list to dict
    }

    private void FormatSprites() {
        confirmBtnSprites = new Dictionary<string, Sprite>(); // Sprite dict
        foreach (Sprite sprite in sprites) { // Look sprites
            confirmBtnSprites.Add(sprite.name, sprite); // Add sprite with name
        }
    }

    // Update sprite from given name in dict
    public void UpdateSprite(string spriteName) {
        confirmCol.enabled = true;
        image.sprite = confirmBtnSprites[spriteName];
        image.enabled = true;
        if(gameMode.danish == false)
        {
            text.SetText(spriteName);
        }
        else
        {
            switch(spriteName)
            {
                case "Move":
                    text.SetText("Flyt");
                    break;
                case "Charge":
                    text.SetText("Mana");
                    break;
                case "Attack":
                    text.SetText("Angrib");
                    break;

            }
        }
        btn.interactable = true;
    }

    public void DisableImage() {
        confirmCol.enabled = false;
        image.enabled = false;
        text.SetText("");
        btn.interactable = false;
    }
}
