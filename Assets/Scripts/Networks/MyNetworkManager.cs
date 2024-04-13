using Unity.Netcode;
using UnityEngine;
using Cinemachine;
public class MyNetworkManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;

    public CinemachineVirtualCamera CinemachineVirtual{get{return mainVirtualCamera;}}
}
