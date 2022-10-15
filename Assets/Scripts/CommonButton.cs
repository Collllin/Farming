using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommonButton : Button
{
    public Action buttonClickAction;
    public Action pointerEnterAction;
    public Action pointerExitAction;
    public Action buttonOnSelectAction;
    public Action buttonOnDeselectAction;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            base.OnPointerClick(eventData);
            buttonClickAction?.Invoke();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        pointerEnterAction?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        pointerExitAction?.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        buttonOnSelectAction?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        buttonOnDeselectAction?.Invoke();
    }
}
