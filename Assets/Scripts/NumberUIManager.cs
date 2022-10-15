using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberUIManager : MonoBehaviour
{
    public GameObject numberPrefab;

    private List<NumberUI> numbers = new();
    private NumberColor numberColor = NumberColor.Red;

    // Start is called before the first frame update
    void Start()
    {
        GameObject numberObj = Instantiate(numberPrefab, transform);
        NumberUI number = numberObj.GetComponent<NumberUI>();
        number.SetColor(numberColor);
        numbers.Add(number);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clear()
    {
        foreach (var num in numbers)
        {
            num.Clear();
        }
    }

    public void SetColor(NumberColor numberColor)
    {
        this.numberColor = numberColor;
        foreach (var number in numbers)
        {
            number.SetColor(numberColor);
        }
    }

    public void ShowNumber(int number)
    {
        foreach (var num in numbers)
        {
            num.Clear();
        }

        if (number == 0)
        {
            numbers[0].ShowNumber(0);
            return;
        }

        int tmpNumber = number;
        int index = 0;
        while (tmpNumber > 0)
        {
            int currentNum = tmpNumber % 10;
            if (numbers.Count <= index)
            {
                Vector3 newPos = new Vector3(numbers[index - 1].transform.position.x - 30, numbers[index - 1].transform.position.y, numbers[index - 1].transform.position.z);
                GameObject numberObj = Instantiate(numberPrefab, newPos, numbers[index - 1].transform.rotation, transform);
                NumberUI num = numberObj.GetComponent<NumberUI>();
                num.SetColor(numberColor);
                numbers.Add(num);
            }
            numbers[index].ShowNumber(currentNum);
            tmpNumber /= 10;
            index++;
        }
    }
}
