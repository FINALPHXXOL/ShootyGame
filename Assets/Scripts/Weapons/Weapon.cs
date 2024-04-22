using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Events")]
    //[HideInInspector] 
    public Pawn owner;
    public UnityEvent OnPrimaryAttackBegin;
    public UnityEvent OnPrimaryAttackEnd;
    public UnityEvent OnSecondaryAttackBegin;
    public UnityEvent OnSecondaryAttackEnd;

    public Transform RightHandIKTarget;
    public Transform LeftHandIKTarget;

    public Sprite weaponIcon;

    public float maxAccuracyRotation;
    

    public virtual float GetAccuracyRotationDegrees()
    {
        // Return a random percentage between min and max AccuracyRoation 
        // Get a random number between 0 and 1 ( a percentage )
        float accuracyDeltaPercentage = Random.value;

        // Find that percentage between the negative (to the Left) and positive (to the right) values of this rotation.
        float accuracyDeltaDegrees = Mathf.Lerp(-maxAccuracyRotation, maxAccuracyRotation, accuracyDeltaPercentage);

        // Return that value
        return accuracyDeltaDegrees;
    }
    public virtual float GetAccuracyRotationDegrees(float accuracyModifier = 1)
    {
        // Return a random percentage between min and max AccuracyRoation 
        // Get a random number between 0 and 1 ( a percentage )
        float accuracyDeltaPercentage = Random.value;

        // Find that percentage between the negative (to the Left) and positive (to the right) values of this rotation.
        float accuracyDeltaDegrees = Mathf.Lerp(-maxAccuracyRotation, maxAccuracyRotation, accuracyDeltaPercentage);
        accuracyDeltaDegrees *= accuracyModifier;

        // Return that value
        return accuracyDeltaDegrees;
    }
}