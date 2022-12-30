using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject attributeBoard;
    [SerializeField] Character character;
    [SerializeField] CellManager cellManager;

    [Header("---Attribute Text---")]
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI seedRegenText;
    [SerializeField] TextMeshProUGUI waterRegenText;
    [SerializeField] TextMeshProUGUI criticalText;
    [SerializeField] TextMeshProUGUI kickBackText;
    [SerializeField] TextMeshProUGUI discountText;
    [SerializeField] TextMeshProUGUI farmingTimeText;
    [SerializeField] TextMeshProUGUI healingTimeText;

    public void PauseGame()
    {
        attributeBoard.SetActive(false);
        GetAttribute();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;   
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        attributeBoard.SetActive(true);
        Time.timeScale = 1;
    }

    public void GetAttribute()
    {
        speedText.text = character.moveSpeed.ToString();
        seedRegenText.text = character.seedRegenerateTime.ToString() + "s";
        waterRegenText.text = character.waterRegenerateTime.ToString() + "s";
        criticalText.text = (character.criticalRate * 100).ToString() + "%";
        kickBackText.text = Mathf.Round((character.kickBackAmount - 1) * 100).ToString() + "%";
        discountText.text = ((1 - character.discountAmount) * 100).ToString() + "%";
        farmingTimeText.text = cellManager.GetFarmingTime().ToString() + "s";
        healingTimeText.text = cellManager.GetHealingTime().ToString() + "s";
    }
}
