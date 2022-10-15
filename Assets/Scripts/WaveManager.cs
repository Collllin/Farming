using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    static private float kWaveTime = 60f;

    public Action gameOverAction;
    public Image progress;
    public Image monthNum;
    public Character character;
    public TextMeshProUGUI storedNumText;
    public CommonInteraction seedTrigger;
    public CommonInteraction storeTrigger;
    public CommonInteraction waterTrigger;
    public CommonInteraction sellTrigger;
    public NumberUIManager coins;
    public GameObject cellsContainer;

    public Sprite[] nums;

    private BasicCell[] cells;
    private Vector3 originalPosition;
    private int waves = 0;
    private int storedNum = 0;
    private int coinNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        cells = cellsContainer.GetComponentsInChildren<BasicCell>();
        coins.ShowNumber(coinNum);

        originalPosition = character.transform.position;
        seedTrigger.interactedAction = () =>
        {
            character.TakeSeed();
        };
        storeTrigger.interactedAction = () =>
        {
            storedNum += character.StorePlant();
            storedNumText.text = storedNum.ToString();
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
        storedNumText.text = "0";
        progress.fillAmount = 0;
        monthNum.sprite = nums[0];
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

    private IEnumerator Countdown()
    {
        float deltaAmount = 1 / kWaveTime;
        while (progress.fillAmount < 1)
        {
            yield return new WaitForEndOfFrame();
            progress.fillAmount += deltaAmount * Time.deltaTime;
        }

        character.ableToMove = false;
        gameOverAction?.Invoke();
    }
}
