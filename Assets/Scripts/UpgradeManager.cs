using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Diagnostics;

public enum UpgradeType
{
    PlantBag = 0,
    SeedBag,
    WaterBag,
    MoveSpeed,
    FarmSpeed,
    SeedRegenTime,
    WaterRegenTime,
    FarmRange,
    CriticalRate,
}

public enum MerchandiseType
{
    UpgradeMap1 = 0,
    UpgradeMap2,
    FreeUpgrade,
    KickbackCard,
    Hoe,
    MembershipCard,
}

public class UpgradeManager : MonoBehaviour
{
    private readonly int[] merchandisePrice = {15, 30, 15, 15, 30, 15};

    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Character character;

    [SerializeField] private Sprite[] upgradeSprites;
    [SerializeField] private Sprite[] storeSprites;

    [SerializeField] private RectTransform choicePrefab;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject refreshButton;
    [SerializeField] private CommonButton skip;
    [SerializeField] private CommonButton refresh;

    [SerializeField] private int refreshCost = 5;
    private List<GameObject> currentChoices = new(); 
    private readonly int upgradeNum = 3;
    private readonly int storeNum = 3;

    private List<UpgradeType> upgradeTypes = new();
    private List<MerchandiseType> merchandiseTypes = new();
    private RectTransform rectTransform;

    private int boughtFieldColumn = 0;

    void Start()
    {
        gameObject.SetActive(false);
        skipButton.SetActive(false);
        rectTransform = GetComponent<RectTransform>();

        waveManager.startUpgradeAction = (completeAction) =>
        {
            gameObject.SetActive(true);

            ShowUpdate(() =>
            {
                foreach (var obj in currentChoices)
                {
                    Destroy(obj);
                }
                currentChoices.Clear();

                ShowStore(() =>
                {
                    gameObject.SetActive(false);

                    foreach (var obj in currentChoices)
                    {
                        Destroy(obj);
                    }
                    currentChoices.Clear();

                    completeAction?.Invoke();
                });
            });
        };
    }

    private void ShowUpdate(Action completeAction)
    {
        upgradeTypes.Clear();
        for (int i = 0; i < upgradeNum; i++)
        {
            float gap = (rectTransform.rect.width - choicePrefab.rect.width * upgradeNum) / (upgradeNum + 1);
            Vector3 pos = new Vector3(choicePrefab.position.x + (i + 1) * gap + (i * 2 + 1) * choicePrefab.rect.width / 2,
                choicePrefab.position.y, choicePrefab.position.z);
            GameObject choiceObj = Instantiate(choicePrefab.gameObject, pos, transform.rotation, transform);
            choiceObj.SetActive(true);
            currentChoices.Add(choiceObj);

            RefreshSingleUpgrade(choiceObj.GetComponent<Image>());

            NumberUIManager uiManager = choiceObj.GetComponentInChildren<NumberUIManager>();
            uiManager.Clear();

            CommonButton button = choiceObj.GetComponent<CommonButton>();
            int index = i;
            button.buttonClickAction = () =>
            {
                Upgrade(upgradeTypes[index]);
                completeAction?.Invoke();
            };
        }
    }

    public void ShowStore(Action completeAction)
    {
        skipButton.SetActive(true);
        skip.buttonClickAction = () =>
        {
            refreshButton.SetActive(false);
            completeAction?.Invoke();
        };
        refreshButton.SetActive(true);
        merchandiseTypes.Clear();
        for (int i = 0; i < storeNum; i++)
        {
            float gap = (rectTransform.rect.width - choicePrefab.rect.width * upgradeNum) / (upgradeNum + 1);
            Vector3 pos = new Vector3(choicePrefab.position.x + (i + 1) * gap + (i * 2 + 1) * choicePrefab.rect.width / 2,
                choicePrefab.position.y, choicePrefab.position.z);
            GameObject choiceObj = Instantiate(choicePrefab.gameObject, pos, transform.rotation, transform);
            choiceObj.SetActive(true);
            currentChoices.Add(choiceObj);

            int index = i;
            RefreshSingleMerchandise(choiceObj.GetComponent<Image>());

            int price = GeneratePrice(merchandiseTypes[index]);
            NumberUIManager uiManager = choiceObj.GetComponentInChildren<NumberUIManager>();
            uiManager.SetColor(NumberColor.Yellow);
            uiManager.ShowNumber(price);

            CommonButton button = choiceObj.GetComponent<CommonButton>();
            button.buttonClickAction = () =>
            {
                Purchase(merchandiseTypes[index], price);
                refreshButton.SetActive(false);
                completeAction?.Invoke();
            };

        }
    }

    void Update()
    {
        
    }

    private void Upgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.FarmSpeed:
                waveManager.IncreaseFarmSpeed();
                break;
            case UpgradeType.MoveSpeed:
                character.IncreaseMoveSpeed();
                break;
            case UpgradeType.PlantBag:
                character.IncreasePlantBag();
                break;
            case UpgradeType.SeedBag:
                character.IncreaseSeedBag();
                break;
            case UpgradeType.WaterBag:
                character.IncreaseWaterBag();
                break;
            case UpgradeType.SeedRegenTime:
                character.DecreaseSeedRegenTime();
                break;
            case UpgradeType.WaterRegenTime:
                character.DecreaseWaterRegenTime();
                break;
            case UpgradeType.FarmRange:
                character.IncreaseFarmRange();
                break;
            case UpgradeType.CriticalRate:
                character.IncreaseCriticalRate();
                break;
        }
    }

    private bool Purchase(MerchandiseType type, int price)
    {
        bool succeed = character.TryPurchase(price);
        if (succeed)
        {
            switch (type)
            {
                case MerchandiseType.UpgradeMap1:
                    boughtFieldColumn += 1;
                    break;
                case MerchandiseType.UpgradeMap2:
                    boughtFieldColumn += 2;
                    break;
            }
        }
        return succeed;
    }

    private int GeneratePrice(MerchandiseType type)
    {
        int price = merchandisePrice[(int)type];
        switch (type)
        {
            case MerchandiseType.UpgradeMap1:
            case MerchandiseType.UpgradeMap2:
                price += boughtFieldColumn * 10;
                break;
        }
        return price;
    }

    private void RefreshSingleUpgrade(Image image)
    {
        bool found = false;
        int tmpIndex = 0;
        while (!found)
        {
            found = true;
            tmpIndex = UnityEngine.Random.Range(0, upgradeSprites.Length);
            foreach (var type in upgradeTypes)
            {
                if ((int)type == tmpIndex)
                {
                    found = false;
                    break;
                }
            }
        }

        upgradeTypes.Add((UpgradeType)tmpIndex);
        image.sprite = upgradeSprites[tmpIndex];
    }

    private void RefreshSingleMerchandise(Image image)
    {
        bool found = false;
        int tmpIndex = 0;
        while (!found)
        {
            found = true;
            tmpIndex = UnityEngine.Random.Range(0, storeSprites.Length);
            foreach (var type in merchandiseTypes)
            {
                if ((int)type == tmpIndex)
                {
                    found = false;
                    break;
                }
            }
        }

        merchandiseTypes.Add((MerchandiseType)tmpIndex);
        image.sprite = storeSprites[tmpIndex];
    }

    public void RefreshStore()
    {
        if (character.coinNum >= refreshCost)
        {
            character.coinNum -= refreshCost;
            character.coins.ShowNumber(character.coinNum);
            refreshCost += 5;
        }
    }
}
