using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    
    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }
}
