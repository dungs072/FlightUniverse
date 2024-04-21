using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ItemSelection : MonoBehaviour
{
    public static event Action OnSelected;
    [SerializeField] private GameObject selectionRing;

    public void ToggleSelectionRing(bool state)
    {
        if(state)
        {
            OnSelected?.Invoke();
            SelectItem();
        }
        selectionRing.SetActive(state);
        
    }
    public virtual void SelectItem()
    {

    }
}
