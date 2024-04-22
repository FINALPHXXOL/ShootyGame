using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : Pickup
{
    public float healAmount;
    public override void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return; // Exit if already triggered

        Pawn thePawn = other.GetComponent<Pawn>();
        if (thePawn != null)
        {
            Health health = thePawn.GetComponent<Health>();
            if (health != null)
            {
                PlayerController pController = thePawn.controller.GetComponent<PlayerController>();
                if (pController != null)
                {
                    isTriggered = true;
                    health.HealDamage(healAmount);
                    // Do what happens with all pickups (from the parent class)
                    base.OnTriggerEnter(other);
                }
            }
        }
    }
}
