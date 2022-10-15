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

    public Sprite[] nums;

    private Transform originalTransform;
    private int waves = 0;
    private int storedNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalTransform = character.transform;
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
        character.transform.localPosition = originalTransform.localPosition;
        character.ableToMove = true;
        character.Reset();

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
