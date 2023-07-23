using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private GameObject healingParticles;
    [SerializeField] private int _health = 100;
    private int maxHealth;

    public int Health { get => _health; set => _health = value; }

    private void Awake()
    {
        maxHealth = _health;
    }

    private void Start()
    {
        healingParticles.SetActive(false);
    }

    public void Heal(int healAmount)
    {
        ShowHealingParticles();
        StartCoroutine(Delay());

        Health += healAmount;

        if (Health > maxHealth)
        {
            Health = maxHealth;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)Health / maxHealth;
    }

    public bool IsDead()
    {
        return Health == 0;
    }

    private void ShowHealingParticles()
    {
        healingParticles.SetActive(true);
    }

    private void HideHealingParticles()
    {
        healingParticles.SetActive(false);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        HideHealingParticles();
    }
}
