using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Values")]
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private float initialHealth;
    [Header("Events")]
    public UnityEvent OnTakeDamage;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    public void Start()
    {
        // Set our current health to our initial health
        currentHealth = initialHealth;
    }

    public void TakeDamage(float damage)
    {
        // Subtract from our current health
        currentHealth -= damage;
        // Make sure we don't go over or under our min and max values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Do any additional functionality
        OnTakeDamage.Invoke();
        // If our health is <= 0, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealDamage(float damage)
    {
        // Add to our current health
        currentHealth += damage;
        // Make sure we don't go over or under our min and max values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Do any additional functionality
        OnHeal.Invoke();
    }

    public void HealToFull()
    {
        // Set our health to max
        currentHealth = maxHealth;
    }

    public void Die()
    {
        // Kill our player
        currentHealth = 0;

        // Do any additional functionality that occurs OnDeath
        OnDeath.Invoke();
    }

    public float HealthPercent()
    {
        // Return current health out of max health (a percent 0 - 1 )
        return currentHealth / maxHealth;
    }
}