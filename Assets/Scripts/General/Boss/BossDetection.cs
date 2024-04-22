using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetection : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private ObjectDetection objectDetection;

    private void Start() {
        objectDetection.OnDetectObj+=UpdateHealthUI;
        objectDetection.OnEnterDetection+=EnterDetection;
        objectDetection.OnExitDetection+=ExitDetection;
    }
    private void EnterDetection()
    {
        UIManager.Instance.ToggleBossHealthBar(true);
        UIManager.Instance.SetHealthBarTitle("Destroyer");
    }
    private void UpdateHealthUI()
    {
        UIManager.Instance.SetBossHealthBarValue(health.CurrentHealth,health.MaxHealth);
    }
    private void ExitDetection()
    {
        UIManager.Instance.ToggleBossHealthBar(false);
    }
    
    
}
