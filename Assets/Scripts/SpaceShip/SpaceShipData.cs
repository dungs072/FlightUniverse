using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpaceShipData", menuName = "SpaceShipData", order = 0)]
public class SpaceShipData : ScriptableObject
{
    [field:SerializeField] public Sprite Avartar{get; private set;}
    [field:SerializeField] public string Title{get;private set;} = "None"; 
}
