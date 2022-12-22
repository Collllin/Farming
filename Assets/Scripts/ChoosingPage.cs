using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//玩家选择角色和植物后才能点击开始游戏按钮
public class ChoosingPage : MonoBehaviour
{
    [SerializeField] bool choseHero = false;
    [SerializeField] bool chosePlant = false;
    [SerializeField] Button startButton;
    
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.enabled = false;
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
            startButton.enabled = true;
        }
    }
}
