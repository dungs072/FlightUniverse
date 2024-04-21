using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipInfo : MonoBehaviour
{
    [field:SerializeField] public MechanicController MechanicController{get;private set;}
    [field:SerializeField] public InfoFighter InfoFighter{get;private set;}
    [field:SerializeField] public SpaceShipData SpaceShipData{get;private set;}
}
