using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;
    public float lives;
    public float score;
    public float accuracy;

    // Start is called before the first frame update
    public virtual void Start()
    {
        // If we were given a pawn at start, possess it
        if (pawn != null)
        {
            PossessPawn(pawn);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.isPaused) return;

            //... the rest of the function goes here.
        }

        // Make decisions
        MakeDecisions();
    }
    /// <summary>
    /// Sets pawn variable to the pawn we want to possess and sets pawn's controller to this controller.
    /// </summary>
    /// <param name="pawnToPossess"></param>
    public virtual void PossessPawn(Pawn pawnToPossess)
    {
         // Set our pawn variable to the pawn we want to possess
         pawn = pawnToPossess;

         // Set the pawn's controller to this controller
         pawn.controller = this;
    }
    /// <summary>
    /// Unlinks both the connected pawn's controller and sets our pawn variable to null.
    /// </summary>
    public virtual void UnpossessPawn()
    {
        // Set the pawn's layer to this controller's layer
        pawn.gameObject.layer = this.gameObject.layer;

        // Set the pawn's controller to this null
        pawn.controller = null;

        // Set our pawn variable to null
        pawn = null;


    }
    protected abstract void MakeDecisions();
    public abstract void Respawn();
    public abstract void AddToScore(float amount);
    public abstract void RemoveScore(float amount);
    public abstract void AddLives(float amount);
    public abstract void RemoveLives(float amount);
}
