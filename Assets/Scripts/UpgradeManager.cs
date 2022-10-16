using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    PlantBag = 0,
    SeedBag,
    WaterBag,
    MoveSpeed,
    FarmSpeed,
}

public class UpgradeManager : MonoBehaviour
{
    public WaveManager waveManager;
    public Character character;
    public CommonInteraction[] upgradeTriggers;
    public SpriteRenderer[] upgradeImages;
    public Sprite[] upgradeSprites;

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

                switch (upgradeTypes[tmpIndex])
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
                }

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

    private void RefreshUpgrade(int index)
    {
        bool found = false;
        int tmpIndex = 0;
        while (!found)
        {
            found = true;
            tmpIndex = Random.Range(0, 5);
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
