using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CellManager : MonoBehaviour
{
    [SerializeField] private GameObject cellsContainer;
    [SerializeField] private GameObject[] cellColumns;
    [SerializeField] private SpriteRenderer fog;
    [SerializeField] private Sprite[] fogSprites;

    private BasicCell[] allCells;
    private List<BasicCell> activeCells;
    private int activedCellColumn = 0;

    // Start is called before the first frame update
    void Start()
    {
        allCells = cellsContainer.GetComponentsInChildren<BasicCell>();
        ResetCells();
    }

    public void ResetCells(bool restart = true)
    {
        foreach (var cell in allCells)
        {
            cell.Reset(restart);
        }

        if (restart)
        {
            BasicCell[] originalCells = cellColumns[0].GetComponentsInChildren<BasicCell>();
            activeCells = new(originalCells);
            fog.sprite = fogSprites[0];
            activedCellColumn = 0;
        }

        foreach (var cell in activeCells)
        {
            cell.SetCellActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseFarmSpeed()
    {
        foreach (var cell in allCells)
        {
            cell.IncreaseFarmSpeed();
        }
    }

    public void DecreaseHealingTime()
    {
        foreach (var cell in allCells)
        {
            cell.DecreaseHealingTime();
        }
    }

    public void ExpandCell()
    {
        if (activedCellColumn < cellColumns.Length - 1)
        {
            activedCellColumn++;

            BasicCell[] tempActivedCellColumn = cellColumns[activedCellColumn].GetComponentsInChildren<BasicCell>();
            foreach (var cell in tempActivedCellColumn)
            {
                activeCells.Add(cell);
                cell.SetCellActive(true);
            }
            fog.sprite = fogSprites[activedCellColumn];
        }
    }
}
