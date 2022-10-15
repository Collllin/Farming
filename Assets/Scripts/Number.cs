using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NumberColor
{
    Red,
    Yellow,
}

public class Number : MonoBehaviour
{
    public Sprite[] redNumbers;
    public Sprite[] yellowNumbers;

    private Sprite[] currentNumber;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currentNumber = yellowNumbers;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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
