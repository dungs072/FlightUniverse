using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectionManager : MonoBehaviour
{
    private List<ItemSelection> itemSelections = new List<ItemSelection>();
    private void Start() {
        
        ItemSelection.OnSelected+=SelectItem;
    }

    private void SelectItem()
    {
        foreach(var item in itemSelections)
        {
            item.ToggleSelectionRing(false);
        }
    }
    public void AddItemSelection(ItemSelection item)
    {
        itemSelections.Add(item);
    }
    public void ClearItemSelection()
    {
        itemSelections.Clear();
    }
}
