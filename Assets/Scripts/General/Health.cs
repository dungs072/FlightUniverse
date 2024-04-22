using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int,int> OnHealthChanged;
    public event Action OnDie;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead;
    public int CurrentHealth{get{return currentHealth;}}
    public int MaxHealth{get{return maxHealth;}}
    private void Awake() {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth-damage, 0);
        OnHealthChanged?.Invoke(currentHealth,maxHealth);
        if(currentHealth==0&&!isDead)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }
}
