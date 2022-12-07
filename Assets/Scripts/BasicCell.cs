using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CellState
{
    Empty,
    Seeded,
    Growing,
    Mature,
    Infecting,
    Infected,
    Healing,
}

public class BasicCell : MonoBehaviour
{
    public NumberManager number;
    public Sprite[] plantSprites;
    public AudioClip[] actionSounds;

    private CellState cellState = CellState.Empty;
    private bool countingDown = false;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float farmingSecond = 5f;
    private float infectionSecond = 10f;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Range"))
        {
            Character character = collision.GetComponentInParent<Character>();
            if (character != null)
            {
                CharacterEnter(character);
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
        infectionSecond = 10;
    }

    public void IncreaseFarmSpeed()
    {
        farmingSecond *= 0.9f;
    }

    private void CharacterEnter(Character character)
    {
        bool shouldSwitchState = false;
        bool harvest = false;

        switch (cellState)
        {
            case CellState.Empty:
                if (character.PlantSeed())
                {
                    shouldSwitchState = true;
                }
                break;
            case CellState.Seeded:
                if (character.WaterPlant())
                {
                    shouldSwitchState = true;
                }
                break;
            case CellState.Mature:
            case CellState.Infecting:
                if (character.Harvest())
                {
                    shouldSwitchState = true;
                    harvest = true;
                }
                break;
            case CellState.Infected:
                if (character.Clean())
                {
                    shouldSwitchState = true;
                }
                break;
        }

        if (shouldSwitchState)
        {
            if (!countingDown || harvest)
            {
                SwitchState(harvest);
            }
        }
    }

    private void SwitchState(bool harvest = false)
    {
        switch (cellState)
        {
            case CellState.Empty:
                cellState = CellState.Seeded;
                spriteRenderer.sprite = plantSprites[0];
                audioSource.clip = actionSounds[0];
                audioSource.Play();
                StartCoroutine(Countdown(farmingSecond));
                break;
            case CellState.Seeded:
                cellState = CellState.Growing;
                spriteRenderer.sprite = plantSprites[1];
                audioSource.clip = actionSounds[1];
                audioSource.Play();
                StartCoroutine(Countdown(farmingSecond, () =>
                {
                    SwitchState();
                }));
                break;
            case CellState.Growing:
                cellState = CellState.Mature;
                spriteRenderer.sprite = plantSprites[2];
                SwitchState();
                break;
            case CellState.Mature:
                if (harvest)
                {
                    Harvest();
                }
                else
                {
                    cellState = CellState.Infecting;
                    StartCoroutine(Countdown(infectionSecond, () =>
                    {
                        SwitchState();
                    }));
                }
                break;
            case CellState.Infecting:
                if (harvest)
                {
                    Harvest();
                }
                else
                {
                    cellState = CellState.Infected;
                    spriteRenderer.sprite = plantSprites[3];
                }
                break;
            case CellState.Infected:
                cellState = CellState.Healing;
                spriteRenderer.sprite = null;
                audioSource.clip = actionSounds[2];
                audioSource.Play();
                StartCoroutine(Countdown(farmingSecond, () =>
                {
                    SwitchState();
                }));
                break;
            case CellState.Healing:
                cellState = CellState.Empty;
                break;
        }
    }

    private IEnumerator Countdown(float seconds, Action countDownCompleteAction = null)
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

        countingDown = false;

        countDownCompleteAction?.Invoke();
    }

    private void Harvest()
    {
        cellState = CellState.Empty;
        spriteRenderer.sprite = null;
        audioSource.clip = actionSounds[2];
        audioSource.Play();
        StopAllCoroutines();
        number.Clear();
        countingDown = false;
    }
}
