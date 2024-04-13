using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicController : MonoBehaviour
{
    [SerializeField] private TrailRenderer forwardTrail;
    [SerializeField] private List<TrailRenderer> backwardTrails;

    public void ToggleForwardTrail(bool state)
    {
        if (!state)
        {
            forwardTrail.Clear();
        }
        forwardTrail.gameObject.SetActive(state);

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
