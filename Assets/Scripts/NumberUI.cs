using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberUI : MonoBehaviour
{
    public Sprite[] redNumbers;
    public Sprite[] yellowNumbers;

    private Sprite[] currentNumber;
    private Image image;

    private void Awake()
    {
        currentNumber = yellowNumbers;
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clear()
    {
        image.sprite = null;
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
        image.sprite = currentNumber[number];
    }
}
