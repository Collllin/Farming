using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CommonButton startButton;
    public GameObject UIOutsideGame;
    public WaveManager waveManager;
    public GameObject UIInGame;

    // Start is called before the first frame update
    void Start()
    {
        UIInGame.SetActive(false);

        startButton.buttonClickAction = () =>
        {
            startButton.gameObject.SetActive(false);
            UIOutsideGame.SetActive(false);
            UIInGame.SetActive(true);
            waveManager.StartGame();
        };

        waveManager.gameOverAction = () =>
        {
            startButton.gameObject.SetActive(true);
            UIOutsideGame.SetActive(true);
            UIInGame.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
