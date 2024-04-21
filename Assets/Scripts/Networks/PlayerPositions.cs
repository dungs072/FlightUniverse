using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositions : MonoBehaviour
{
    [SerializeField] private List<PlayerInitializationData> startPositions;
    private List<PlayerInitializationData> positions;
    private void Start() {
        positions = new List<PlayerInitializationData>(startPositions);
    }
    public PlayerInitializationData GetRandomPlayerData()
    {
        if(positions.Count == 0)
        {
            return null;    
        }
        PlayerInitializationData position = positions[Random.Range(0, positions.Count)];
        positions.Remove(position);
        return position;
    }
}
