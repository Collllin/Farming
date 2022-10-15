using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Character : MonoBehaviour
{
    public Action<Transform> characterPosChanged;

    public float moveSpeed = 1;
    [HideInInspector]
    public bool ableToMove = false;

    public TextMeshProUGUI seedNum;
    public TextMeshProUGUI waterNum;
    public TextMeshProUGUI plantNum;

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
    }

    public void TakeSeed()
    {
        seedAmount = seedLimitation;
        seedNum.text = seedAmount.ToString();
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

    public void TakeWater()
    {
        waterAmount = waterLimitation;
        waterNum.text = waterAmount.ToString();
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
}
