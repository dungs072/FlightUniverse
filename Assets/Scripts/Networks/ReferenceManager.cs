using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    
    [SerializeField] private List<AISpaceShipController> ais;
    [SerializeField] private List<AIShooterController> aIShooterControllers;
    [SerializeField] private RocketShooter rocketShooter;
    public static ReferenceManager Instance{get; private set;}
    private GameObject player;
    public GameObject Player{get{return player;}}
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DontDestroyOnLoad(Instance);
        }
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;   
    }
    public void SetAITarget(Transform target)
    {
        foreach(var ai in ais)
        {
            ai.SetTarget(target);
        }
        foreach(var ai in aIShooterControllers)
        {
            ai.AddPlayers(target.GetComponent<SpaceShipController>());
        }
        rocketShooter.SetTarget(target);
    }
    // private void Start() {
    //     NetworkManager.Singleton.ConnectionApprovalCallback=ApprovalCheck;
    // }
    // private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    // {
    //     response.Approved = true;
    //     var player = NetworkManager.Singleton.LocalClient.PlayerObject;
    //     print(player);
    //     foreach(var ai in ais)
    //     {
    //         ai.SetTarget(player.transform);
    //     }
    // }
}
