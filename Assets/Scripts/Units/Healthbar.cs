using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private GameMode gameMode;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthText;
    public GameObject healthbarAssembly;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameMode = gameManager.GetComponent<GameMode>();
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        UpdateHealthText();
        fill.color = gradient.Evaluate(1f);
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if(gameMode.danish == false)
        {
            healthText.text = "Health: " + slider.value + "/" + slider.maxValue;
        }
        else
        {
            healthText.text = "Liv: " + slider.value + "/" + slider.maxValue;
        }
        
        if (slider.value == 0)
        {
            healthText.enabled = false;
            healthbarAssembly.SetActive(false);
        }
    }
}
