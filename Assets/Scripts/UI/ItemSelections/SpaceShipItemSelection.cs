using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class SpaceShipItemSelection : ItemSelection
{
    public static event Action<SpaceShipData> OnSpaceShipItemSelect;
    [SerializeField] private Image avatar;
    [SerializeField] private TMP_Text title;
    private SpaceShipData spaceShipData;
    public SpaceShipData SpaceShipData { get { return spaceShipData; } }

    private void SetDisplayData() {
        avatar.sprite = spaceShipData.Avartar;
        title.text = spaceShipData.Title;   
    }
    public void SetSpaceShipData(SpaceShipData data)
    {
        spaceShipData = data;
        SetDisplayData();
    }
    public override void SelectItem()
    {
        OnSpaceShipItemSelect?.Invoke(spaceShipData);
    }
}
