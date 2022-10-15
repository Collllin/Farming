using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CommonInteraction : MonoBehaviour
{
    public Action interactedAction;

    public SpriteRenderer indicator;

    private bool ableToInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        indicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ableToInteract)
        {
            if (Input.GetButtonDown("Interact"))
            {
                indicator.color = Color.green;
            }
            else if (Input.GetButtonUp("Interact"))
            {
                indicator.color = Color.yellow;
                interactedAction?.Invoke();
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
