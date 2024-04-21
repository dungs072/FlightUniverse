using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.VFX;

public class PlayerInitialization : NetworkBehaviour
{
    private readonly string WarpAmount = "WarpAmount";
    [SerializeField] private float lightSpeed = 50f;
    [SerializeField] private float fadeoutLightSpeedVFX = 2f;
    [SerializeField] private float shortestDistance = 2f;
    [SerializeField] private float nearDistance = 1f;

    [SerializeField] private SpaceShipController spaceShipController;
    [SerializeField] private VisualEffect lightSpeedVFX;
    private PlayerPositions playerPositions;
    private PlayerInitializationData data;
    private float currentSpeed;
    private void Start()
    {
        currentSpeed = lightSpeed;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        playerPositions = ReferenceManager.Instance.GetComponent<PlayerPositions>();
        data = playerPositions.GetRandomPlayerData();
        if (data == null) { return; }
        transform.position = data.StartPosition.position;
        transform.rotation = data.StartPosition.rotation;
        UIManager.Instance.ToggleSelectionSpaceShipPanel(true);
        ReferenceManager.Instance.SetPlayer(gameObject);

    }
    public void StartGame()
    {
        UIManager.Instance.TogglePlayGamePanel(false);
        spaceShipController.MechanicController.ToggleForwardTrail(true);
        spaceShipController.CanControl = false;
        StartCoroutine(StartMoveInLightSpeed());
    }
    private IEnumerator StartMoveInLightSpeed()
    {
        float value = 1f;
        
        while (!IsInDestination(data.DestinationPosition.position))
        {
            yield return null;
            transform.position += transform.forward * Time.deltaTime * currentSpeed;
            Vector3 direction = (data.DestinationPosition.position - transform.position).normalized;
            Quaternion quaternion = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 5f);
            if (IsNearDestination(data.DestinationPosition.position))
            {
                value = Mathf.Max(value - Time.deltaTime * fadeoutLightSpeedVFX, 0f);
                currentSpeed -= 0.2f;
                if (currentSpeed <= 0f)
                {
                    break;
                }
                lightSpeedVFX.SetFloat(WarpAmount, value);
            }
        }
        spaceShipController.CanControl = true;
        lightSpeedVFX.SetFloat(WarpAmount, 0);
        spaceShipController.MechanicController.ToggleForwardTrail(false);
        UIManager.Instance.TogglePlayGamePanel(true);

        // set cursor here
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    private bool IsInDestination(Vector3 destination)
    {
        return (destination - transform.position).sqrMagnitude <= shortestDistance * shortestDistance;
    }
    private bool IsNearDestination(Vector3 destination)
    {
        return (destination - transform.position).sqrMagnitude <= nearDistance * nearDistance;
    }

}
