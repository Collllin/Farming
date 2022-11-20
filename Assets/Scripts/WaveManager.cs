using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    private float kWaveTime = 60f;
    private static readonly int kUpgradePrice = 20;

    public Character character;
    public GameObject cellsContainer;
    //public UpgradeManager upgradeManager;

    [Header("---- UI ----")]
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] GameObject storeMenu;
    public Action gameOverAction;
    public Image progress;
    public NumberUIManager monthNum;
    public GameObject bigMonth;
    public NumberUIManager bigMonthNum;
    public NumberUIManager storedNumText;
    public NumberUIManager goalNumText;
    public NumberUIManager coins;
    public Sprite[] nums;

    [Header("---- Trigger ----")]
    public CommonInteraction storeTrigger;
    public CommonInteraction sellTrigger;

    [Header("---- Sound ----")]
    public AudioClip[] backgroundMusic;

    private BasicCell[] cells;
    private Vector3 originalPosition;
    private int months = 0;
    private int storedNum = 0;
    private int coinNum = 0;
    const int   basicGoalNum = 10;
    private int goalNum = 20;
    private int lastGoalNum = 20;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        cells = cellsContainer.GetComponentsInChildren<BasicCell>();
        bigMonth.SetActive(false);
        upgradeMenu.SetActive(false);
        storeMenu.SetActive(false);

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

        storeTrigger.interactedAction = () =>
        {
            int addNum = character.StorePlant();
            storedNum += addNum;
            storedNumText.ShowNumber(storedNum);
            return addNum != 0;
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
        lastGoalNum = 20;
        goalNum = 20;
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

            goalNum = lastGoalNum + (int)(Math.Sqrt(months)) * basicGoalNum;
            lastGoalNum = goalNum;

            goalNumText.ShowNumber(goalNum);

            kWaveTime = 60;
            audioSource.clip = backgroundMusic[0];
            audioSource.Play();

            progress.fillAmount = 0;
            StartCoroutine(Countdown());
        }
    }

    public void StartUpgrade()
    {
        Time.timeScale = 0;
        upgradeMenu.SetActive(true);
    }

    public void EndUpgrade()
    {
        Time.timeScale = 1;
        upgradeMenu.SetActive(false);
    }

    public void OpenStore()
    {
        Time.timeScale = 0;
        storeMenu.SetActive(true);
    }

    public void CloseStore()
    {
        Time.timeScale = 1;
        storeMenu.SetActive(false);
    }

    private IEnumerator ShowBigMonth()
    {
        bigMonth.SetActive(true);
        bigMonthNum.ShowNumber(months, 130);
        yield return new WaitForSeconds(2);
        bigMonth.SetActive(false);
    }
}
