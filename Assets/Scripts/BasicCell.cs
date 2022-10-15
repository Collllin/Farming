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
    public Sprite[] numbers;

    private SpriteRenderer spriteRenderer;

    private CellState cellState = CellState.Empty;
    private bool countingDown = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
                break;
            case CellState.Seeded:
                if (!character.WaterPlant())
                {
                    shouldCountdown = false;
                }
                break;
            case CellState.Mature:
                character.Harvest();
                shouldCountdown = false;
                cellState = CellState.Empty;
                break;
        }

        if (shouldCountdown)
        {
            countingDown = true;

            spriteRenderer.sprite = numbers[5];
            yield return new WaitForSeconds(1);
            spriteRenderer.sprite = numbers[4];
            yield return new WaitForSeconds(1);
            spriteRenderer.sprite = numbers[3];
            yield return new WaitForSeconds(1);
            spriteRenderer.sprite = numbers[2];
            yield return new WaitForSeconds(1);
            spriteRenderer.sprite = numbers[1];
            yield return new WaitForSeconds(1);
            spriteRenderer.sprite = null;

            switch (cellState)
            {
                case CellState.Empty:
                    cellState = CellState.Seeded;
                    break;
                case CellState.Seeded:
                    cellState = CellState.Mature;
                    break;
                case CellState.Mature:
                    break;
            }

            countingDown = false;
        }
    }
}
