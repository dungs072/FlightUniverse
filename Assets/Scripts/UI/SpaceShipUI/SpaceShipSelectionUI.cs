using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSelectionUI : MonoBehaviour
{
    [SerializeField] private List<SpaceShipData> spaceShipDatas;
    [SerializeField] private Transform content;
    [SerializeField] private SpaceShipItemSelection spaceShipItemSelectionPrefab;
    [SerializeField] private ItemSelectionManager itemSelectionManager;

    private SpaceShipData currentSelectSpaceShipData;
    public SpaceShipData CurrentSelectSpaceShipData{get{return currentSelectSpaceShipData;}}
    private void Start() {
        CreateSelection();
        SpaceShipItemSelection.OnSpaceShipItemSelect+=SelectSpaceship;
    }
    private void CreateSelection()
    {
        foreach(var data in spaceShipDatas)
        {
            var instance = Instantiate(spaceShipItemSelectionPrefab,content);
            instance.SetSpaceShipData(data);
            itemSelectionManager.AddItemSelection(instance);
        }
    }
    private void SelectSpaceship(SpaceShipData data)
    {
        currentSelectSpaceShipData = data;
    }


}
