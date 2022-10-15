using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Empty,
    Seeded,
    Mature,
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

    // Start is called before the first frame update
    void Start()
    {
        number.SetColor(NumberColor.Red);
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!countingDown)
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                StartCoroutine(Countdown(character));
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
    }

    private IEnumerator Countdown(Character character)
    {
        bool shouldCountdown = true;
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
                    spriteRenderer.sprite = null;
                    audioSource.clip = actionSounds[2];
                    audioSource.Play();
                    cellState = CellState.Empty;
                }
                shouldCountdown = false;
                break;
        }

        if (shouldCountdown)
        {
            countingDown = true;

            number.ShowNumber(5);
            yield return new WaitForSeconds(1);
            number.ShowNumber(4);
            yield return new WaitForSeconds(1);
            number.ShowNumber(3);
            yield return new WaitForSeconds(1);
            number.ShowNumber(2);
            yield return new WaitForSeconds(1);
            number.ShowNumber(1);
            yield return new WaitForSeconds(1);
            number.Clear();
            switch (cellState)
            {
                case CellState.Empty:
                    cellState = CellState.Seeded;
                    break;
                case CellState.Seeded:
                    cellState = CellState.Mature;
                    spriteRenderer.sprite = plantSprites[2];
                    break;
                case CellState.Mature:
                    break;
            }

            countingDown = false;
        }
    }
}
