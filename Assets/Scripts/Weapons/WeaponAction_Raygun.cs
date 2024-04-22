using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction_Raygun : WeaponAction
{
    public float fireDistance;
    public Transform firePoint;
    public float damageDone;
    public float fireRate;

    private bool isAutofireActive;
    private LineRenderer lineRenderer;
    private float lastShotTime;
    public GameObject projectilePrefab;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (isAutofireActive)
        {
            if (GameManager.instance.isPaused) return;

            //... the rest of the function goes here.
            Shoot();
        }
    }
    
    public void Shoot()
    {
        // Store the direction we shoot without the accuracy system
        Vector3 newFireDirection = firePoint.forward;

        // Get the roation change based on our accuracy
        Quaternion accuracyFireDelta = Quaternion.Euler(0, weapon.GetAccuracyRotationDegrees(weapon.owner.controller.accuracy), 0);

        // Multiply by the rotation from inaccuacy to set new rotation value
        // (Note that multiplication between a Quaternion and Vector is not commutative. The order matters!)
        newFireDirection = accuracyFireDelta * newFireDirection;

        // Create a variable to hold our raycast hit data
        RaycastHit hit;

        // Check if it is time to fire our weapon 
        float secondsPerShot = 1 / fireRate;
        if (Time.time >= lastShotTime + secondsPerShot)
        {
            // if so...
            // if so, do the Raycast
            if (Physics.Raycast(firePoint.position, newFireDirection, out hit, fireDistance))
            {
                {
                    // Instantiate the projectile
                    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation) as GameObject;
                    LaserBeam newProjectile = projectile.GetComponent<LaserBeam>();

                    shotFiredSound.Play();
                    muzzleParticle.SpawnParticles();
                    // If we hit, and the other object has a Health component
                    Health otherHealth = hit.collider.gameObject.GetComponent<Health>();
                    if (otherHealth != null)
                    {
                        // Tell it to take damage!
                        otherHealth.TakeDamage(damageDone);
                    }
                    else
                    {
                        // If we didn't hit, draw the visuals the full distance of the raycast
                        LaserBeam visuals = projectile.GetComponent<LaserBeam>();
                        visuals.startPoint = firePoint.position;
                        visuals.endPoint = firePoint.position + (newFireDirection * fireDistance);
                    }

                    newProjectile.startPoint = firePoint.position;
                    newProjectile.endPoint = hit.collider.transform.position;

                    // Set the layer for the projectile
                    projectile.gameObject.layer = this.gameObject.layer;

                    /* Set the data for the projectile
                    Projectile projectileData = projectile.GetComponent<Projectile>();
                    if (projectileData != null)
                    {
                        projectileData.damage = damageDone;
                    }
                    */
                    // Save the time we shot
                    lastShotTime = Time.time;
                }
            }

        }
    }

    public void AutofireBegin()
    {
        isAutofireActive = true;
    }

    public void AutofireEnd()
    {
        isAutofireActive = false;
    }
}