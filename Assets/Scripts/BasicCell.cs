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
    public Number number;

    private CellState cellState = CellState.Empty;
    private bool countingDown = false;

    // Start is called before the first frame update
    void Start()
    {
        number.SetColor(NumberColor.Yellow);
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
                    break;
                case CellState.Mature:
                    break;
            }

            countingDown = false;
        }
    }
}
