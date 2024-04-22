using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    //[HideInInspector] 
    public NavMeshAgent agent;
    public float stoppingDistance;
    public Transform targetTransform;
    private Vector3 desiredVelocity = Vector3.zero;
    public float shootingDistance;
    public float shootingAngle;

    // Start is called before the first frame update
    public override void Start()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.enemyAISpawnTransform != null)
            {
                GameManager.instance.enemies.Add(this);
            }
        }
        base.Start();
    }

    // OnDestroy is called before object is destroyed.
    public void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.enemyAISpawnTransform != null)
            {
                GameManager.instance.enemies.Remove(this);
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        // If we don't have a target, target the player (if we can)
        if (!HasTarget())
        {
            TargetPlayer();
        }

        // Do what all controllers do each frame (Make Decisions)
        base.Update();
    }
    /// <summary>
    /// Links with pawn's NavMesh component and adds controller to pawn while adding pawn to controller.
    /// Also sets various variables.
    /// </summary>
    /// <param name="pawnToPossess"></param>
    public override void PossessPawn(Pawn pawnToPossess)
    {
        // Set the variables (from the base class's definition)
        base.PossessPawn(pawnToPossess);

        // Get the agent off the pawn
        agent = pawn.GetComponent<NavMeshAgent>();
     
        // If it doesn't have one, add one and store it
        if (agent == null)
        {
            agent = pawn.gameObject.AddComponent<NavMeshAgent>();
        }

        // Set the stopping distance
        agent.stoppingDistance = stoppingDistance;

        // Set the max speed of the AI from the pawn data
        agent.speed = pawn.maxMoveSpeed;

        // Set the max rotation speed of the AI from the pawn data
        agent.angularSpeed = pawn.maxRotationSpeed;

        // Disable movement and rotation from the NavMeshAgent
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    private void ShootTarget()
    {
        if (pawn != null)
        {
            // If we are within distance
            if (Vector3.Distance(targetTransform.position, pawn.transform.position) <= shootingDistance)
            {
                // And we are within the angle
                Vector3 vectorToTarget = targetTransform.position - pawn.transform.position;
                if (Vector3.Angle(pawn.transform.forward, vectorToTarget) <= shootingAngle)
                {
                    if (pawn != null)
                    {
                        if (pawn.weapon != null)
                        {
                            // They should pull the trigger
                            pawn.weapon.OnPrimaryAttackBegin.Invoke();
                        }
                    }
                }
            }
            else
            {
                if (pawn != null)
                {
                    if (pawn.weapon != null)
                    {
                        // They can release the trigger
                        pawn.weapon.OnPrimaryAttackEnd.Invoke();
                    }
                }  
            }
        }
    }

    private bool HasTarget()
    {
        if (targetTransform != null)
        {
            ShootTarget();
            return true;
        }
        return false;
    }

    private void TargetPlayer()
    {
        if (pawn != null)
        {
            Controller playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                if (playerController.pawn != null)
                {
                    targetTransform = playerController.pawn.transform;
                }
            }
        }
    }

    /// <summary>
    /// Destroys ties with AI NavMesh and unlinks pawn's controller and this pawn.
    /// </summary>
    public override void UnpossessPawn()
    {
        // Remove the NavMeshAgent
        Destroy(agent);

        // Set the variables (from the base class's definition)
        base.UnpossessPawn();
    }

    /// <summary>
    /// Performs all functions that AIController makes on Update()
    /// </summary>
    protected override void MakeDecisions()
    {
        if (pawn != null)
        {
            // If we don't have a pawn, we can't make decisions for it, so do nothing
            if (pawn == null)
            {
                return;
            }

            // Set our NavMeshAgent to seek our target
            agent.SetDestination(targetTransform.position);

            // Find the velocity that the agent wants to move in order to follow the path
            desiredVelocity = agent.desiredVelocity;

            // Send the direction in to our Move function (use the move function to add speed)
            pawn.Move(desiredVelocity.normalized);

            // Look towards the player
            pawn.RotateToLookAt(targetTransform.position);
        }
    }
    public override void Respawn()
    {

    }
    public override void AddToScore(float amount)
    {

    }
    public override void RemoveScore(float amount)
    {

    }
    public override void AddLives(float amount)
    {

    }
    public override void RemoveLives(float amount)
    {

    }
    public void ProcessInputs()
    {

    }
}