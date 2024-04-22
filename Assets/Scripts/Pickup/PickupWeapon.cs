using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : Pickup
{
    public Weapon weaponToEquip;

    public override void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return; // Exit if already triggered
  
        // Equip the weapon
        if (weaponToEquip != null)
        {
            Pawn thePawn = other.GetComponent<Pawn>();
            if (thePawn != null)
            {
                PlayerController pController = thePawn.controller.GetComponent<PlayerController>();
                if (pController != null)
                {
                    isTriggered = true;
                    thePawn.EquipWeapon(weaponToEquip);
                    base.OnTriggerEnter(other);
                }
            }
        }
    }
}
