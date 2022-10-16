using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    private float kWaveTime = 60f;
    private static readonly int kUpgradePrice = 20;

    public Action gameOverAction;
    public Image progress;
    public NumberUIManager monthNum;
    public GameObject bigMonth;
    public NumberUIManager bigMonthNum;
    public Character character;
    public UpgradeManager upgradeManager;
    public NumberUIManager storedNumText;
    public NumberUIManager goalNumText;
    public CommonInteraction seedTrigger;
    public CommonInteraction storeTrigger;
    public CommonInteraction waterTrigger;
    public CommonInteraction sellTrigger;
    public NumberUIManager coins;
    public GameObject cellsContainer;
    public AudioClip[] backgroundMusic;

    public Sprite[] nums;

    private BasicCell[] cells;
    private Vector3 originalPosition;
    private int months = 0;
    private int storedNum = 0;
    private int coinNum = 0;
    private int goalNum = 10;
    private int lastGoalNum = 10;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        cells = cellsContainer.GetComponentsInChildren<BasicCell>();
        bigMonth.SetActive(false);

        coins.SetColor(NumberColor.Yellow);
        coins.ShowNumber(coinNum);
        monthNum.SetColor(NumberColor.Yellow);
        monthNum.ShowNumber(months);
        bigMonthNum.SetColor(NumberColor.Yellow);
        bigMonthNum.ShowNumber(months);
        storedNumText.SetColor(NumberColor.Red);
        storedNumText.ShowNumber(storedNum);
        goalNumText.SetColor(NumberColor.Red);
        goalNumText.ShowNumber(goalNum);

        originalPosition = character.transform.position;
        seedTrigger.interactedAction = () =>
        {
            return character.TakeSeed();
        };
        storeTrigger.interactedAction = () =>
        {
            int addNum = character.StorePlant();
            storedNum += addNum;
            storedNumText.ShowNumber(storedNum);
            return addNum != 0;
        };
        waterTrigger.interactedAction = () =>
        {
            return character.TakeWater();
        };
        sellTrigger.interactedAction = () =>
        {
            int addNum = character.StorePlant();
            coinNum += addNum;
            coins.ShowNumber(coinNum);
            return addNum != 0;
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
        lastGoalNum = 10;
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

        upgradeManager.GetFreeUpdate(false);
        StartCoroutine(ShowBigMonth());
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

            StartCoroutine(ShowBigMonth());

            if (months % 4 == 3)
            {
                lastGoalNum = goalNum;
                goalNum = 0;
                goalNumText.ShowNumber(goalNum);
                kWaveTime = 30;
                audioSource.clip = backgroundMusic[1];
                audioSource.Play();
                upgradeManager.GetFreeUpdate(true);
            }
            else
            {
                goalNum = lastGoalNum;
                goalNum += 20;
                lastGoalNum = goalNum;
                goalNumText.ShowNumber(goalNum);
                kWaveTime = 60;
                audioSource.clip = backgroundMusic[0];
                audioSource.Play();
                upgradeManager.GetFreeUpdate(false);
            }

            progress.fillAmount = 0;
            StartCoroutine(Countdown());
        }
    }

    private IEnumerator ShowBigMonth()
    {
        bigMonth.SetActive(true);
        bigMonthNum.ShowNumber(months, 130);
        yield return new WaitForSeconds(2);
        bigMonth.SetActive(false);
    }
}
