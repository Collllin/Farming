using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    public Sprite[] redNumbers;
    public Sprite[] yellowNumbers;

    private Sprite[] currentNumber;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    public void Clear()
    {
        spriteRenderer.sprite = null;
    }

    public void SetColor(NumberColor numberColor)
    {
        switch (numberColor)
        {
            case NumberColor.Red:
                currentNumber = redNumbers;
                break;
            case NumberColor.Yellow:
                currentNumber = yellowNumbers;
                break;
        }
    }

    public void ShowNumber(int number)
    {
        spriteRenderer.sprite = currentNumber[number];
    }
}
