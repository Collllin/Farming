using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Character : MonoBehaviour
{
    public Action<Transform> characterPosChanged;

    public float moveSpeed = 10;
    [HideInInspector]
    public bool ableToMove = false;

    public TextMeshProUGUI seedNum;
    public TextMeshProUGUI waterNum;
    public TextMeshProUGUI plantNum;
    public TextMeshProUGUI seedLimitationText;
    public TextMeshProUGUI waterLimitationText;
    public TextMeshProUGUI plantLimitationText;

    private int seedLimitation = 10;
    private int waterLimitation = 10;
    private int plantLimitation = 15;

    private int seedAmount = 0;
    private int waterAmount = 0;
    private int plantAmount = 0;

    private Rigidbody2D rBody;

    private float speed;
    private float vspeed;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!ableToMove)
        {
            rBody.velocity = Vector2.zero;
            return;
        }

        int leftRightInput = 0;
        int upDownInput = 0;

        //Debug.Log(Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Horizontal") == 1)
        {
            leftRightInput = 1;
        }
        else if (Input.GetAxis("Horizontal") == -1)
        {
            leftRightInput = -1;
        }

        if (Input.GetAxis("Vertical") == 1)
        {
            upDownInput = 1;
        }
        else if (Input.GetAxis("Vertical") == -1)
        {
            upDownInput = -1;
        }

        if (leftRightInput != 0)
        {
            speed = moveSpeed * leftRightInput;
        }
        else
        {
            speed = 0;
        }

        if (upDownInput != 0)
        {
            vspeed = moveSpeed * upDownInput;
        }
        else
        {
            vspeed = 0;
        }

        rBody.velocity = new Vector2(speed, vspeed);

        characterPosChanged?.Invoke(transform);

        //if (speed != 0 || vspeed != 0)
        //{
        //    if (walk)
        //    {
        //        characterAnimator.SetTrigger("Walk");
        //    }
        //    else
        //    {
        //        characterAnimator.SetTrigger("Run");
        //    }
        //    if (!footStepSource.isPlaying)
        //    {
        //        footStepSource.Play();
        //    }
        //}
        //else
        //{
        //    characterAnimator.SetTrigger("Stand");
        //    footStepSource.Stop();
        //}
    }

    public void Reset()
    {
        seedAmount = 0;
        seedNum.text = "0";
        waterAmount = 0;
        waterNum.text = "0";
        plantAmount = 0;
        plantNum.text = "0";
        moveSpeed = 10f;
        plantLimitation = 15;
        plantLimitationText.text = plantLimitation.ToString();
        seedLimitation = 10;
        seedLimitationText.text = seedLimitation.ToString();
        waterLimitation = 10;
        waterLimitationText.text = waterLimitation.ToString();
    }

    public bool TakeSeed()
    {
        if (seedAmount == seedLimitation)
        {
            return false;
        }
        else
        {
            seedAmount = seedLimitation;
            seedNum.text = seedAmount.ToString();
            return true;
        }
    }

    public bool PlantSeed()
    {
        if (seedAmount > 0)
        {
            seedAmount--;
            seedNum.text = seedAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TakeWater()
    {
        if (waterAmount == waterLimitation)
        {
            return false;
        }
        else
        {
            waterAmount = waterLimitation;
            waterNum.text = waterAmount.ToString();
            return true;
        }
    }

    public bool WaterPlant()
    {
        if (waterAmount > 0)
        {
            waterAmount--;
            waterNum.text = waterAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Harvest()
    {
        if (plantAmount < plantLimitation)
        {
            plantAmount++;
            plantNum.text = plantAmount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int StorePlant()
    {
        int storedNum = plantAmount;
        plantAmount = 0;
        plantNum.text = "0";
        return storedNum;
    }

    public void IncreaseMoveSpeed()
    {
        moveSpeed *= 1.15f;
    }

    public void IncreasePlantBag()
    {
        plantLimitation += 15;
        plantLimitationText.text = plantLimitation.ToString();
    }

    public void IncreaseSeedBag()
    {
        seedLimitation += 10;
        seedLimitationText.text = seedLimitation.ToString();
    }

    public void IncreaseWaterBag()
    {
        waterLimitation += 10;
        waterLimitationText.text = waterLimitation.ToString();
    }
}
