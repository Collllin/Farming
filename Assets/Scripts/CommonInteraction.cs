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
                indicator.color = Color.blue;
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
                indicator.color = Color.white;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            indicator.color = Color.white;
            indicator.gameObject.SetActive(true);
            ableToInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            indicator.gameObject.SetActive(false);
            ableToInteract = false;
        }
    }
}
