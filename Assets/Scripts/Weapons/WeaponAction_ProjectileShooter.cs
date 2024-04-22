using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction_ProjectileShooter : WeaponAction
{
    public float damageDone;
    public float fireRate;

    private float lastShotTime;
    public Transform firePoint;
    public GameObject projectilePrefab;

    public void Shoot()
    {
        // Store the direction we shoot without the accuracy system
        Vector3 newFireDirection = firePoint.forward;

        // Get the roation change based on our accuracy
        Quaternion accuracyFireDelta = Quaternion.Euler(0, weapon.GetAccuracyRotationDegrees(weapon.owner.controller.accuracy), 0);

        // Multiply by the rotation from inaccuacy to set new rotation value
        // (Note that multiplication between a Quaternion and Vector is not commutative. The order matters!)
        newFireDirection = accuracyFireDelta * newFireDirection;

        // Check if it is time to fire our weapon 
        float secondsPerShot = 1 / fireRate;
        if (Time.time >= lastShotTime + secondsPerShot)
        {
            // if so...

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation) as GameObject;

            shotFiredSound.Play();
            muzzleParticle.SpawnParticles();

            // Rotate it based on accuracy
            projectile.transform.Rotate(0, weapon.GetAccuracyRotationDegrees(weapon.owner.controller.accuracy), 0);

            // Set the layer for the projectile
            projectile.gameObject.layer = this.gameObject.layer;

            // Set the data for the projectile
            Projectile projectileData = projectile.GetComponent<Projectile>();
            if (projectileData != null)
            {
                projectileData.damage = damageDone;
            }

            // Save the time we shot
            lastShotTime = Time.time;
        }
    }
}
