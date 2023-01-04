using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private CellManager cellManager;

    [Header("---- UI ----")]
    public int timeLimit = 60;
    public NumberUIManager timeLimitNum;
    public NumberUIManager monthNum;
    public GameObject bigMonth;
    public NumberUIManager bigMonthNum;
    public NumberUIManager storedNumText;
    public NumberUIManager goalNumText;
    public GameObject pauseButton;
    
    public Sprite[] nums;

    [Header("---- Refresh ----")]
    [SerializeField] private int upgradeRefreshCost;
    [SerializeField] private int storeRefreshCost;
    const int defaultRefreshCost = 5;

    [Header("---- Trigger ----")]
    public CommonInteraction storeTrigger;
    public CommonInteraction sellTrigger;

    [Header("---- Sound ----")]
    public AudioClip[] backgroundMusic;

    public Action gameOverAction;
    public Action<Action> startUpgradeAction;

    private Vector3 originalPosition;
    private int months = 1;
    private int storedNum = 0;
    const double basicGoalNum = 10;
    private int goalNum = 20;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        bigMonth.SetActive(false);

        timeLimitNum.SetColor(NumberColor.Red);
        timeLimitNum.ShowNumber(timeLimit);
        monthNum.SetColor(NumberColor.Yellow);
        monthNum.ShowNumber(months);
        bigMonthNum.SetColor(NumberColor.Yellow);
        bigMonthNum.ShowNumber(months);
        storedNumText.SetColor(NumberColor.Red);
        storedNumText.ShowNumber(storedNum);
        goalNumText.SetColor(NumberColor.Red);
        goalNumText.ShowNumber(goalNum);

        originalPosition = character.transform.position;

        storeTrigger.interactedAction = () =>
        {
            int addNum = character.StorePlant();
            storedNum += addNum;
            storedNumText.ShowNumber(storedNum);
            return addNum != 0;
        };
        sellTrigger.interactedAction = () =>
        {
            return character.SellPlants();
        };
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            storedNum = goalNum;
            timeLimit = 0;
        }
    }

    public void StartGame()
    {
        timeLimit = 60;
        storedNum = 0;
        storedNumText.ShowNumber(storedNum);
        goalNum = 20;
        goalNumText.ShowNumber(goalNum);
        months = 1;
        monthNum.ShowNumber(months);
        upgradeRefreshCost = defaultRefreshCost;
        storeRefreshCost = defaultRefreshCost;
        character.transform.position = originalPosition;
        character.ableToMove = true;
        character.Reset();
        cellManager.ResetCells();

        StartCoroutine(ShowBigMonth());
        StartCoroutine(Countdown());
    }

    public void IncreaseFarmSpeed()
    {
        cellManager.IncreaseFarmSpeed();
    }

    private IEnumerator Countdown()
    {
        while (timeLimit >= 0)
        {
            yield return new WaitForSeconds(1f);
            timeLimit--;
            timeLimitNum.ShowNumber(timeLimit);
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

            character.ableToMove = false;
            character.ResetAfterMonth();
            cellManager.ResetCells(false);
            pauseButton.SetActive(false);
            StartUpgrade(() =>
            {
                upgradeRefreshCost = defaultRefreshCost;
                storeRefreshCost = defaultRefreshCost;

                months++;
                monthNum.ShowNumber(months);

                StartCoroutine(ShowBigMonth());

                double goal = Math.Log(months, 3.0) * basicGoalNum;
                goalNum += (int)goal;

                goalNumText.ShowNumber(goalNum);

                audioSource.clip = backgroundMusic[0];
                audioSource.Play();

                timeLimit = 60;
                StartCoroutine(Countdown());

                pauseButton.SetActive(true);
                character.ableToMove = true;
            });
        }
    }

    private void StartUpgrade(Action upgradeComplete)
    {
        startUpgradeAction?.Invoke(() =>
        {
            upgradeComplete?.Invoke();
        });
    }

    private IEnumerator ShowBigMonth()
    {
        bigMonth.SetActive(true);
        bigMonthNum.ShowNumber(months, 130);
        yield return new WaitForSeconds(2);
        bigMonth.SetActive(false);
    }

    public bool RefreshUpgrade()
    {
        if (character.coinNum >= upgradeRefreshCost)
        {
            character.coinNum -= upgradeRefreshCost;
            character.coins.ShowNumber(character.coinNum);
            upgradeRefreshCost += defaultRefreshCost;

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RefreshStore()
    {
        if (character.coinNum >= storeRefreshCost)
        {
            character.coinNum -= storeRefreshCost;
            character.coins.ShowNumber(character.coinNum);
            storeRefreshCost += defaultRefreshCost;

            return true;
        }
        else
        {
            return false;
        }
    }
}
