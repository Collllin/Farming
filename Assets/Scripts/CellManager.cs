using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private GameObject cellsContainer;
    [SerializeField] private GameObject[] cellColumns;
    [SerializeField] private SpriteRenderer fog;
    [SerializeField] private Sprite[] fogSprites;

    private BasicCell[] allCells;
    private List<BasicCell> activeCells;

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
            cell.Reset();
        }

        if (restart)
        {
            BasicCell[] originalCells = cellColumns[0].GetComponentsInChildren<BasicCell>();
            activeCells = new(originalCells);
            fog.sprite = fogSprites[0];
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
}
