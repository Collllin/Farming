using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    static private float kWaveTime = 60f;
    private static int kUpgradePrice = 30;

    public Action gameOverAction;
    public Image progress;
    public NumberUIManager monthNum;
    public Character character;
    public NumberUIManager storedNumText;
    public NumberUIManager goalNumText;
    public CommonInteraction seedTrigger;
    public CommonInteraction storeTrigger;
    public CommonInteraction waterTrigger;
    public CommonInteraction sellTrigger;
    public NumberUIManager coins;
    public GameObject cellsContainer;

    public Sprite[] nums;

    private BasicCell[] cells;
    private Vector3 originalPosition;
    private int months = 0;
    private int storedNum = 0;
    private int coinNum = 0;
    private int goalNum = 10;

    // Start is called before the first frame update
    void Start()
    {
        cells = cellsContainer.GetComponentsInChildren<BasicCell>();

        coins.SetColor(NumberColor.Yellow);
        coins.ShowNumber(coinNum);
        monthNum.SetColor(NumberColor.Yellow);
        monthNum.ShowNumber(months);
        storedNumText.SetColor(NumberColor.Red);
        storedNumText.ShowNumber(storedNum);
        goalNumText.SetColor(NumberColor.Red);
        goalNumText.ShowNumber(goalNum);

        originalPosition = character.transform.position;
        seedTrigger.interactedAction = () =>
        {
            character.TakeSeed();
        };
        storeTrigger.interactedAction = () =>
        {
            storedNum += character.StorePlant();
            storedNumText.ShowNumber(storedNum);
        };
        waterTrigger.interactedAction = () =>
        {
            character.TakeWater();
        };
        sellTrigger.interactedAction = () =>
        {
            coinNum += character.StorePlant();
            coins.ShowNumber(coinNum);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        storedNum = 0;
        storedNumText.ShowNumber(storedNum);
        goalNum = 10;
        goalNumText.ShowNumber(goalNum);
        progress.fillAmount = 0;
        months = 0;
        monthNum.ShowNumber(months);
        character.transform.position = originalPosition;
        character.ableToMove = true;
        character.Reset();
        foreach (var cell in cells)
        {
            cell.Reset();
        }
        coinNum = 0;
        coins.ShowNumber(coinNum);

        StartCoroutine(Countdown());
    }

    public bool TryPurchase()
    {
        if (coinNum >= kUpgradePrice)
        {
            coinNum -= kUpgradePrice;
            coins.ShowNumber(coinNum);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void IncreaseFarmSpeed()
    {
        foreach (var cell in cells)
        {
            cell.IncreaseFarmSpeed();
        }
    }

    private IEnumerator Countdown()
    {
        float deltaAmount = 1 / kWaveTime;
        while (progress.fillAmount < 1)
        {
            yield return new WaitForEndOfFrame();
            progress.fillAmount += deltaAmount * Time.deltaTime;
        }

        if (storedNum < goalNum)
        {
            character.ableToMove = false;
            gameOverAction?.Invoke();
        }
        else
        {
            storedNum = 0;
            storedNumText.ShowNumber(storedNum);

            months++;
            monthNum.ShowNumber(months);

            goalNum += months * 10;
            goalNumText.ShowNumber(goalNum);

            progress.fillAmount = 0;
            StartCoroutine(Countdown());
        }
    }
}
