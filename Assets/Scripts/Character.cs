using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.TextCore.Text;
using Unity.Mathematics;

public class Character : MonoBehaviour
{
    public Action<Transform> characterPosChanged;

    [HideInInspector]
    public bool ableToMove = false;

    public TextMeshProUGUI seedNum;
    public TextMeshProUGUI waterNum;
    public TextMeshProUGUI plantNum;
    public TextMeshProUGUI seedLimitationText;
    public TextMeshProUGUI waterLimitationText;
    public TextMeshProUGUI plantLimitationText;
    public NumberUIManager coins;
    public Transform farmRange;

    public int coinNum = 0;

    [Header("---Show in Pause Menu---")]
    public float moveSpeed = 10;
    public int seedRestoreAmount = 1;
    public int waterRestoreAmount = 1;
    public float seedRegenerateTime = 0.75f;
    public float waterRegenerateTime = 0.75f;
    public float criticalRate = 0;
    public int criticalAmount = 1;
    public float kickBackAmount = 1;
    public float discountAmount = 1;

    private Rigidbody2D rBody;

    private int seedLimitation = 10;
    private int waterLimitation = 10;
    private int plantLimitation = 15;

    private int seedAmount = 10;
    private int waterAmount = 10;
    private int plantAmount = 0;

    private float speed;
    private float vspeed;
    private float originalScale;

    private bool seedRestoring = false;
    private bool waterRestoring = false;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        Vector3 scale = farmRange.localScale;
        originalScale = scale.x;
        coins.SetColor(NumberColor.Yellow);
        coins.ShowNumber(coinNum);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.Equals))
        {
            coinNum += 888;
            coins.ShowNumber(coinNum);
        }

        if (!ableToMove)
        {
            rBody.velocity = Vector2.zero;
            return;
        }

        int leftRightInput = 0;
        int upDownInput = 0;

        //Debug.Log(Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Horizontal") == 1)
        {
            leftRightInput = 1;
        }
        else if (Input.GetAxis("Horizontal") == -1)
        {
            leftRightInput = -1;
        }

        if (Input.GetAxis("Vertical") == 1)
        {
            upDownInput = 1;
        }
        else if (Input.GetAxis("Vertical") == -1)
        {
            upDownInput = -1;
        }

        if (leftRightInput != 0)
        {
            speed = moveSpeed * leftRightInput;
        }
        else
        {
            speed = 0;
        }

        if (upDownInput != 0)
        {
            vspeed = moveSpeed * upDownInput;
        }
        else
        {
            vspeed = 0;
        }

        rBody.velocity = new Vector2(speed, vspeed);

        characterPosChanged?.Invoke(transform);

        CheckSeedRestoration();
        CheckWaterRestoration();
    }

    public void Reset()
    {
        seedAmount = 10;
        seedNum.text = "10";
        waterAmount = 10;
        waterNum.text = "10";
        plantAmount = 0;
        plantNum.text = "0";
        moveSpeed = 10f;
        plantLimitation = 15;
        plantLimitationText.text = plantLimitation.ToString();
        seedLimitation = 10;
        seedLimitationText.text = seedLimitation.ToString();
        waterLimitation = 10;
        waterLimitationText.text = waterLimitation.ToString();
        coinNum = 0;
        coins.ShowNumber(coinNum);

        Vector3 newScale = new(originalScale, originalScale, 0);
        farmRange.localScale = newScale;

        seedRegenerateTime = 0.75f;
        waterRegenerateTime = 0.75f;

        StopAllCoroutines();
        waterRestoring = false;
        seedRestoring = false;
    }

    public void ResetAfterMonth()
    {
        seedAmount = seedLimitation;
        seedNum.text = seedAmount.ToString();
        waterAmount = waterLimitation;
        waterNum.text = waterAmount.ToString();
        plantAmount = 0;
        plantNum.text = "0";
        StopAllCoroutines();
        waterRestoring = false;
        seedRestoring = false;
    }

    public bool SellPlants()
    {
        int addNum = StorePlant();
        coinNum += (int)(addNum * kickBackAmount);
        coins.ShowNumber(coinNum);
        return addNum != 0;
    }

    public bool TryPurchase(int price)
    {
        if (coinNum >= (int)(price * discountAmount))
        {
            coinNum -= (int)(price * discountAmount);
            coins.ShowNumber(coinNum);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlantSeed()
    {
        if (seedAmount > 0)
        {
            seedAmount--;
            seedNum.text = seedAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckSeedRestoration()
    {
        if (seedRestoring || seedAmount >= seedLimitation)
        {
            return;
        }

        StartCoroutine(RestoreSeed());
    }

    private IEnumerator RestoreSeed()
    {
        seedRestoring = true;
        yield return new WaitForSeconds(seedRegenerateTime);
        seedAmount += seedRestoreAmount;
        seedNum.text = seedAmount.ToString();
        seedRestoring = false;
    }

    public bool WaterPlant()
    {
        if (waterAmount > 0)
        {
            waterAmount--;
            waterNum.text = waterAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckWaterRestoration()
    {
        if (waterRestoring || waterAmount >= waterLimitation)
        {
            return;
        }

        StartCoroutine(RestoreWater());
    }

    private IEnumerator RestoreWater()
    {
        waterRestoring = true;
        yield return new WaitForSeconds(waterRegenerateTime);
        waterAmount += waterRestoreAmount;
        waterNum.text = waterAmount.ToString();
        waterRestoring = false;
    }

    public bool Harvest()
    {
        float tempRand = UnityEngine.Random.Range(0f,1f);
        if (plantAmount < plantLimitation)
        {
            if (tempRand < criticalRate)
            {
                plantAmount += 1 + criticalAmount;

                if (plantAmount >= plantLimitation)
                {
                    plantAmount = plantLimitation;
                }
            }
            else
            {
                plantAmount++;
            }
            plantNum.text = plantAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Clean()
    {
        return true;
    }

    public int StorePlant()
    {
        int storedNum = plantAmount;
        plantAmount = 0;
        plantNum.text = "0";
        return storedNum;
    }

    public void IncreaseMoveSpeed()
    {
        moveSpeed *= 1.15f;
    }

    public void IncreasePlantBag()
    {
        plantLimitation += 15;
        plantLimitationText.text = plantLimitation.ToString();
    }

    public void IncreaseSeedBag()
    {
        seedLimitation += 10;
        seedAmount = seedLimitation;
        seedLimitationText.text = seedLimitation.ToString();
        seedNum.text = seedAmount.ToString();
    }

    public void IncreaseWaterBag()
    {
        waterLimitation += 10;
        waterAmount = waterLimitation;
        waterLimitationText.text = waterLimitation.ToString();
        waterNum.text = waterAmount.ToString();
    }

    public void DecreaseSeedRegenTime()
    {
        seedRegenerateTime -= 0.15f;
    }

    public void DecreaseWaterRegenTime()
    {
        waterRegenerateTime -= 0.15f;
    }

    public void IncreaseFarmRange()
    {
        Vector3 scale = farmRange.localScale;
        Vector3 newScale = new(scale.x * 1.15f, scale.y * 1.15f, scale.z);
        farmRange.localScale = newScale;
    }

    public void IncreaseCriticalRate()
    {
        criticalRate += 0.05f;
    }

    public void IncreaseCriticalAmount()
    {
        criticalAmount++;
    }

    public void GetDiscount()
    {
        discountAmount -= 0.05f;
    }

    public void GetKickBack()
    {
        kickBackAmount += 0.05f;
    }
}
