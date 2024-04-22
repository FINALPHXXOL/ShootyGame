using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionSpawnParticles : GameAction
{
    public GameObject particlePrefab;
    public float lifespan;
    public Transform targetNode;
    [HideInInspector]
    public GameObject currentParticle;
    public bool followPosition = false;

    public override void Update()
    {
        if (followPosition == true)
        {
            if (currentParticle != null)
            {
                currentParticle.transform.position = particlePrefab.transform.position;
            }
        }
    }

    public void SpawnParticles()
    {
        // Instantiate the particles at the target node
        currentParticle = Instantiate<GameObject>(particlePrefab, targetNode.position, targetNode.rotation);
        // if there is a lifespan, set the particle system to destroy
        if (lifespan > 0)
        {
            Destroy(currentParticle, lifespan);
        }
    }
}
