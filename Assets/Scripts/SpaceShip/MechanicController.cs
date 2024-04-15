using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MechanicController : NetworkBehaviour
{
    [SerializeField] private TrailRenderer forwardTrail;
    [SerializeField] private List<TrailRenderer> backwardTrails;

    public void HandleToggleForwardTrail(bool state)
    {
        ToggleForwardTrail(state);
        ToggleForwardTrailServerRpc(state);
    }
    private void ToggleForwardTrail(bool state)
    {
        if (!state)
        {
            forwardTrail.Clear();
        }
        forwardTrail.gameObject.SetActive(state);
    }
    public void HandleToggleBackwardTrails(bool state)
    {
        ToggleBackwardTrails(state);
        ToggleBackwardTrailsServerRpc(state);
    }
    private void ToggleBackwardTrails(bool state)
    {
        foreach (var trail in backwardTrails)
        {
            if (!state)
            {
                trail.Clear();
            }
            trail.gameObject.SetActive(state);

        }
    }
    [ServerRpc]
    private void ToggleForwardTrailServerRpc(bool state)
    {
        ToggleForwardTrailClientRpc(state);
    }
    [ClientRpc]
    private void ToggleForwardTrailClientRpc(bool state)
    {
        if (IsOwner) { return; }
        ToggleForwardTrail(state);
    }
    [ServerRpc]
    private void ToggleBackwardTrailsServerRpc(bool state)
    {
        ToggleBackwardTrailsClientRpc(state);
    }
    [ClientRpc]
    private void ToggleBackwardTrailsClientRpc(bool state)
    {
        if(IsOwner) { return; }
        ToggleBackwardTrails(state);
    }
}
