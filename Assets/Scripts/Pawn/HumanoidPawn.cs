using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidPawn : Pawn
{
    public Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // Get the animator attached to this object
        animator = GetComponent<Animator>();
    }
    public Animator GetAnimator()
    {
        // return our private variable
        return animator;
    }

    /// <summary>
    /// Starts sprint and changes maxMoveSpeed to sprintSpeed.
    /// </summary>
    public override void Sprint()
    {
        maxMoveSpeed = sprintSpeed;
        isSprinting = true;
    }
    /// <summary>
    /// Stops sprint. And resets maxMoveSpeed to default.
    /// </summary>
    public override void StopSprint()
    {
        maxMoveSpeed = defaultSpeed;
        isSprinting = false;
    }
    /// <summary>
    /// Moves pawn in direction with respect to assigned maxMoveSpeed.
    /// </summary>
    /// <param name="direction"></param>
    public override void Move(Vector3 direction)
    {
        // Apply the speed to the direction vector by muliplying by our max speed
        direction = direction * maxMoveSpeed;

        // Send the values from the direction in to the animator
        animator.SetFloat("Forward", direction.z);
        animator.SetFloat("Right", direction.x);
    }
    public override void Rotate(float speed)
    {
        // Use the Rotate function to rotate based on speed
        transform.Rotate(0, speed * maxRotationSpeed * Time.deltaTime, 0);
    }
    /// <summary>
    /// Rotates AI Pawn to face target.
    /// </summary>
    /// <param name="targetPoint"></param>
    public override void RotateToLookAt(Vector3 targetPoint)
    {
        // Find the vector from our position to the target point
        Vector3 lookVector = targetPoint - transform.position;

        // Find the rotation that will look down that vector with world up being the up direction
        Quaternion lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        // Rotate slightly towards that target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, maxRotationSpeed * Time.deltaTime);
    }
    /// <summary>
    /// Uses root motion to effect AI Pawn's position.
    /// </summary>
    public void OnAnimatorMove()
    {
        // After the animation runs
        // Use root motion to move the game object
        transform.position = animator.rootPosition;
        transform.rotation = animator.rootRotation;

        // If we have a NavMeshAgent on our controller,
        AIController aiController = controller as AIController;
        if (aiController != null)
        {
            // Set our navMeshAgent to understand it is as the position from the animator
            aiController.agent.nextPosition = animator.rootPosition;
        }
    }

    public void OnAnimatorIK()
    {
        // If they don't have a weapon, don't worry about IK
        if (!weapon)
        {
            // Set their IK weights to 0
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);

            // Leave the function
            return;
        }

        // Set the IK for our Right Hand
        if (weapon.RightHandIKTarget)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, weapon.RightHandIKTarget.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.RightHand, weapon.RightHandIKTarget.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        }

        // Set the IK for our Left LeftHandIKTarget)
        if (weapon.LeftHandIKTarget)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, weapon.LeftHandIKTarget.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, weapon.LeftHandIKTarget.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        }
    }
}
