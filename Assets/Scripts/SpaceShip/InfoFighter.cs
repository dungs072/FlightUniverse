using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoFighter : MonoBehaviour
{
    [field: SerializeField] public List<Transform> SpawnProjectilePos { get; private set; }
    [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    [field:SerializeField] public float FireRate{get;private set;} = 0.3f;
}
