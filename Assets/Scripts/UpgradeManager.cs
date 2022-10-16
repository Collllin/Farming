using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    PlantBag = 0,
    SeedBag,
    WaterBag,
    MoveSpeed,
    FarmSpeed,
    FarmRange,
}

public class UpgradeManager : MonoBehaviour
{
    public WaveManager waveManager;
    public Character character;
    public CommonInteraction[] upgradeTriggers;
    public SpriteRenderer[] upgradeImages;
    public Sprite[] upgradeSprites;
    public Image freeUpgradeImage;

    private UpgradeType[] upgradeTypes = { UpgradeType.PlantBag, UpgradeType.SeedBag, UpgradeType.WaterBag };

    // Start is called before the first frame update
    void Start()
    {
        RefreshUpgrade(0);
        RefreshUpgrade(1);
        RefreshUpgrade(2);

        for (int i = 0; i < 3; i++)
        {
            CommonInteraction trigger = upgradeTriggers[i];
            int tmpIndex = i;
            trigger.interactedAction = () =>
            {
                if (!waveManager.TryPurchase())
                {
                    return false;
                }

                Upgrade(upgradeTypes[tmpIndex]);

                upgradeTypes[tmpIndex] = (UpgradeType)999;
                RefreshUpgrade(tmpIndex);

                return true;
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetFreeUpdate(bool real)
    {
        if (real)
        {
            int tmpIndex = Random.Range(0, 6);
            Upgrade((UpgradeType)tmpIndex);
            freeUpgradeImage.color = new Color(freeUpgradeImage.color.r, freeUpgradeImage.color.g, freeUpgradeImage.color.b, 1);
            freeUpgradeImage.sprite = upgradeSprites[tmpIndex];
            AudioSource audio = freeUpgradeImage.GetComponent<AudioSource>();
            audio.Play();
        }
        else
        {
            freeUpgradeImage.color = new Color(freeUpgradeImage.color.r, freeUpgradeImage.color.g, freeUpgradeImage.color.b, 0);
        }
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
            case UpgradeType.FarmRange:
                character.IncreaseFarmRange();
                break;
        }
    }

    private void RefreshUpgrade(int index)
    {
        bool found = false;
        int tmpIndex = 0;
        while (!found)
        {
            found = true;
            tmpIndex = Random.Range(0, 6);
            foreach (var type in upgradeTypes)
            {
                if ((int)type == tmpIndex)
                {
                    found = false;
                    break;
                }
            }
        }

        upgradeTypes[index] = (UpgradeType)tmpIndex;
        upgradeImages[index].sprite = upgradeSprites[tmpIndex];
    }
}
