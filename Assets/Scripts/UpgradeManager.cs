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
    ExpandCellSmall = 0,
    ExpandCellBig,
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
    [SerializeField] private CellManager cellManager;

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

    private bool getFreeUpgrade = false;
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
                ClearSavedChoices();

                ShowStore(() =>
                {
                    ClearSavedChoices();
                    gameObject.SetActive(false);

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
        refresh.buttonClickAction = () =>
        {
            if (RefreshStore())
            {
                ClearSavedChoices();
                ShowStore(() =>
                {
                    completeAction?.Invoke();
                });
            };
        };
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
                if (Purchase(merchandiseTypes[index], price))
                {
                    refreshButton.SetActive(false);
                    if (getFreeUpgrade)
                    {
                        ClearSavedChoices();
                        ShowUpdate(() =>
                        {
                            ClearSavedChoices();
                            completeAction?.Invoke();
                        });
                        getFreeUpgrade = false;
                    }
                    else
                    {
                        completeAction?.Invoke();
                    }
                }
            };

        }
    }

    void Update()
    {
        
    }

    private void ClearSavedChoices()
    {
        foreach (var obj in currentChoices)
        {
            Destroy(obj);
        }
        currentChoices.Clear();
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
        if (character.TryPurchase((int)(price * character.discountAmount)))
        {
            switch (type)
            {
                case MerchandiseType.ExpandCellSmall:
                    boughtFieldColumn += 1;
                    cellManager.ExpandCell();
                    break;
                case MerchandiseType.ExpandCellBig:
                    boughtFieldColumn += 2;
                    cellManager.ExpandCell();
                    cellManager.ExpandCell();
                    break;
                case MerchandiseType.FreeUpgrade:
                    getFreeUpgrade = true;
                    break;
                case MerchandiseType.Hoe:
                    cellManager.DecreaseHealingTime();
                    break;
                case MerchandiseType.KickbackCard:
                    character.GetKickBack();
                    break;
                case MerchandiseType.MembershipCard:
                    character.GetDiscount();
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GeneratePrice(MerchandiseType type)
    {
        int price = (int)(merchandisePrice[(int)type] * character.discountAmount);
        switch (type)
        {
            case MerchandiseType.ExpandCellSmall:
            case MerchandiseType.ExpandCellBig:
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

    public bool RefreshStore()
    {
        if (character.coinNum >= refreshCost)
        {
            character.coinNum -= refreshCost;
            character.coins.ShowNumber(character.coinNum);
            refreshCost += 5;

            return true;
        }
        else 
        {
            return false;
        }
    }
}
