using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializationData : MonoBehaviour
{
    [field:SerializeField] public Transform StartPosition{get;private set;}
    [field:SerializeField] public Transform DestinationPosition{get;private set;}
}
