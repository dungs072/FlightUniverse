using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MechanicController : MonoBehaviour
{
    [SerializeField] private List<TrailRenderer> forwardTrails;
    [SerializeField] private List<TrailRenderer> backwardTrails;

    public void HandleToggleForwardTrail(bool state)
    {
        ToggleForwardTrail(state);
    }
    public void ToggleForwardTrail(bool state)
    {

        foreach (var trail in forwardTrails)
        {
            if (!state)
            {
                trail.Clear();
            }
            trail.gameObject.SetActive(state);
        }

    }
    public void HandleToggleBackwardTrails(bool state)
    {
        ToggleBackwardTrails(state);
    }
    public void ToggleBackwardTrails(bool state)
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
  
}
