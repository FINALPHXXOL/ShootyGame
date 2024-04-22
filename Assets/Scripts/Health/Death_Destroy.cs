using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a Health Component
[RequireComponent(typeof(Health))]
public class Death_Destroy : GameAction
{
    [SerializeField] private float delayBeforeDestruction;

    public override void Start()
    {
        // Get the health component
        Health health = GetComponent<Health>();

        // Register with the OnDie event
        health.OnDeath.AddListener(DestroyOnDeath);
        base.Start();
    }

    public void DestroyOnDeath()
    {
        Destroy(gameObject, delayBeforeDestruction);
    }
}

// In the Pawn