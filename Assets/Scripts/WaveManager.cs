using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    private static readonly float kWaveTime = 60f;

    [SerializeField] private Character character;
    [SerializeField] private CellManager cellManager;

    [Header("---- UI ----")]
    public Image progress;
    public NumberUIManager monthNum;
    public GameObject bigMonth;
    public NumberUIManager bigMonthNum;
    public NumberUIManager storedNumText;
    public NumberUIManager goalNumText;
    
    public Sprite[] nums;

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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        bigMonth.SetActive(false);

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            storedNum = goalNum;
            progress.fillAmount = 1;
        }
    }

    public void StartGame()
    {
        storedNum = 0;
        storedNumText.ShowNumber(storedNum);
        goalNum = 20;
        goalNumText.ShowNumber(goalNum);
        progress.fillAmount = 0;
        months = 1;
        monthNum.ShowNumber(months);
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

            character.ableToMove = false;
            character.ResetAfterMonth();
            cellManager.ResetCells(false);
            StartUpgrade(() =>
            {
                months++;
                monthNum.ShowNumber(months);

                StartCoroutine(ShowBigMonth());

                double goal = Math.Log(months, 3.0) * basicGoalNum;
                goalNum += (int)goal;

                goalNumText.ShowNumber(goalNum);

                audioSource.clip = backgroundMusic[0];
                audioSource.Play();

                progress.fillAmount = 0;
                StartCoroutine(Countdown());

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
}
