using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum CellState
{
    Empty,
    Seeded,
    Mature,
    Infected,
    Healing,
}

public class BasicCell : MonoBehaviour
{
    public NumberManager number;
    public Sprite[] plantSprites;
    public AudioClip[] actionSounds;

    private CellState cellState = CellState.Empty;
    private bool shouldCountdown = true;
    private bool countingDown = false;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float farmingSecond = 5f;
    private float infectedSecond = 10f;

    // Start is called before the first frame update
    void Start()
    {
        number.SetColor(NumberColor.Yellow);
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!countingDown && collision.CompareTag("Range"))
        {
            Character character = collision.GetComponentInParent<Character>();
            if (character != null)
            {
                CharacterEnter(character);
                StartCoroutine(Countdown(farmingSecond));
            }
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
        cellState = CellState.Empty;
        countingDown = false;
        number.Clear();
        spriteRenderer.sprite = null;
        farmingSecond = 5;
        infectedSecond = 10;
    }

    public void IncreaseFarmSpeed()
    {
        farmingSecond *= 0.9f;
    }

    private void CharacterEnter(Character character)
    {
        shouldCountdown = true;

        switch (cellState)
        {
            case CellState.Empty:
                if (!character.PlantSeed())
                {
                    shouldCountdown = false;
                }
                else
                {
                    spriteRenderer.sprite = plantSprites[0];
                    audioSource.clip = actionSounds[0];
                    audioSource.Play();
                }
                break;
            case CellState.Seeded:
                if (!character.WaterPlant())
                {
                    shouldCountdown = false;
                }
                else
                {
                    spriteRenderer.sprite = plantSprites[1];
                    audioSource.clip = actionSounds[1];
                    audioSource.Play();
                }
                break;
            case CellState.Mature:
                if (character.Harvest())
                {
                    shouldCountdown = false;
                    spriteRenderer.sprite = null;
                    audioSource.clip = actionSounds[2];
                    audioSource.Play();
                    cellState = CellState.Empty;
                }
                break;
            case CellState.Infected:
                if (character.Clean())
                {
                    audioSource.clip = actionSounds[0];
                    audioSource.Play();
                    spriteRenderer.sprite = null;
                    cellState = CellState.Healing;
                }
                break;
        }
    }

    private IEnumerator Countdown(float seconds)
    {
        bool stateChangeIndicater = false;

        if (shouldCountdown)
        {
            countingDown = true;

            int integerSeconds = (int)seconds;
            float tmpSeconds = seconds - integerSeconds;

            number.ShowNumber(integerSeconds);
            yield return new WaitForSeconds(tmpSeconds);

            while (integerSeconds > 0)
            {
                number.ShowNumber(integerSeconds);
                yield return new WaitForSeconds(1);
                integerSeconds--;
            }

            number.Clear();
            switch (cellState)
            {
                case CellState.Empty:
                    cellState = CellState.Seeded;
                    break;
                case CellState.Seeded:
                    cellState = CellState.Mature;
                    spriteRenderer.sprite = plantSprites[2];
                    stateChangeIndicater = true;
                    break;
                case CellState.Mature:
                    cellState = CellState.Infected;
                    spriteRenderer.sprite = plantSprites[3];
                    break;
                case CellState.Healing:
                    cellState = CellState.Empty;
                    break;
            }

            countingDown = false;

            if (stateChangeIndicater)
            {
                StartCoroutine(Countdown(infectedSecond));
            }
        }
    }
}
