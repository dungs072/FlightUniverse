using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoFighter : MonoBehaviour
{
    [field: SerializeField] public List<Transform> SpawnProjectilePos { get; private set; }
    [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    [field:SerializeField] public List<LineRenderer> LaserLines{get;private set;}
    [field:SerializeField] public float FireRate{get;private set;} = 0.3f;
    [field:SerializeField] public float LaserLength{get;private set;} = 100f;

    [field:SerializeField] public LayerMask ObstacleLayer{get; private set; }

    // begin sounds
    [field:SerializeField] public AudioClip RifleShootSound;
    // end sounds

}
