using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate bool InteractionDelegate();

public class CommonInteraction : MonoBehaviour
{
    public InteractionDelegate interactedAction;

    public SpriteRenderer indicator;
    public AudioClip[] audioClips;

    private bool ableToInteract = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        indicator.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ableToInteract)
        {
            if (Input.GetButtonDown("Interact"))
            {
                indicator.color = Color.green;
                if (interactedAction())
                {
                    audioSource.clip = audioClips[0];
                }
                else
                {
                    audioSource.clip = audioClips[1];
                }
                audioSource.Play();
            }
            else if (Input.GetButtonUp("Interact"))
            {
                indicator.color = Color.yellow;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        indicator.color = Color.yellow;
        indicator.gameObject.SetActive(true);
        ableToInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        indicator.gameObject.SetActive(false);
        ableToInteract = false;
    }
}
