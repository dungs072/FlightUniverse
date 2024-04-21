using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using Unity.VisualScripting;
public class PlayerData : NetworkBehaviour
{
    [SerializeField] private SpaceShipController spaceShipController;
    [SerializeField] private Fighter fighter;

    [SerializeField] private List<SpaceShipInfo> infos;

    public void SetUpSpaceShipData(SpaceShipData data)
    {
        foreach(var i in infos)
        {
            i.gameObject.SetActive(false);
            if(i.SpaceShipData==data)
            {
                spaceShipController.SetMechanicController(i.MechanicController);
                fighter.SetInfoFighter(i.InfoFighter);

                i.gameObject.SetActive(true);
            }
            
        }
    }
}
