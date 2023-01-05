using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingPage : MonoBehaviour
{
    [SerializeField] bool choseHero = false;
    [SerializeField] bool chosePlant = false;
    [SerializeField] GameObject startButton;
    [SerializeField] Sprite startSprite_Unable;
    [SerializeField] Sprite startSprite_Enable;
    
    void Start()
    {
        startButton.GetComponent<Image>().sprite = startSprite_Unable;
        startButton.GetComponent<CommonButton>().interactable = false;
        choseHero = false;
        chosePlant = false;
    }

    public void ChooseAHero()
    {
        choseHero = true;
    }

    public void ChooseAPlant()
    {
        chosePlant = true;
    }

    void Update()
    {
        if (choseHero && chosePlant)
        {
            startButton.GetComponent<CommonButton>().interactable = true;
            startButton.GetComponent<Image>().sprite = startSprite_Enable;
        }
    }
}
