using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [Header("Key Inputs")]
    [SerializeField, Tooltip("Key that is used to initiate and deinitiate sprint.")]
    private KeyCode sprintKey;

    [Header("Booleans")]
    [SerializeField, Tooltip("Do you want camera rotation via mouse position?")]
    public bool isMouseRotation;

    // Start is called before the first frame update
    public override void Start()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.playerSpawnTransform != null)
            {
                GameManager.instance.players.Add(this);
            }
        }

        base.Start();
    }
    // OnDestroy is called before object is destroyed.
    public void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.playerSpawnTransform != null)
            {
                GameManager.instance.players.Remove(this);
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (pawn != null)
        {
            if (GameManager.instance.isPaused) return;

            //... the rest of the function goes here.
            ProcessInputs();
        }
    }
    /// <summary>
    /// Performs all functions that AIController makes on Update()
    /// </summary>
    protected override void MakeDecisions()
    {
        if (pawn != null)
        {
            // Get the Input axes
            Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Limit the vector to a max magnitude of 1
            moveVector = Vector3.ClampMagnitude(moveVector, 1);
            // Convert our direction from world space (as passed in) to local space (that we need for the Animator)
            moveVector = transform.InverseTransformDirection(moveVector);
            // Tell the pawn to move
            pawn.Move(moveVector);

            // if we are mouse rotating
            if (isMouseRotation)
            {
                // Create the Ray from the mouse position in the direciton the camera is facing.
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Create a plane at our feet, and a normal (perpendicular direction) of world up.
                Plane footPlane = new Plane(Vector3.up, pawn.transform.position);

                // Find the distance down that ray that the plane and ray intersect
                float distanceToIntersect;
                // Out arguments don't have to be initialized before being passed in a method call.
                // However, the called method is required to assign a value before the method returns.            
                if (footPlane.Raycast(mouseRay, out distanceToIntersect))
                {
                    // Find the intersection point
                    Vector3 intersectionPoint = mouseRay.GetPoint(distanceToIntersect);

                    // Look at the intersection point
                    pawn.RotateToLookAt(intersectionPoint);
                }
                else
                {
                    Debug.Log("Camera is not looking at the ground - no intersection between plane and ray");
                }
            }
            else
            {
                // Tell the pawn to rotate based on the CameraRotation axis
                pawn.Rotate(Input.GetAxis("CameraRotation"));
            }
        }
    }
    /// <summary>
    /// Processes player inputs and completes processes.
    /// </summary>
    public void ProcessInputs()
    {
        if (Input.GetKeyDown(sprintKey))
        {
            pawn.Sprint();
        }
        if (Input.GetKeyUp(sprintKey))
        {
            pawn.StopSprint();
        }
        // Call Weapon Events on FireButton presses
        if (Input.GetButtonDown("Fire1"))
        {
            pawn.weapon.OnPrimaryAttackBegin.Invoke();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            pawn.weapon.OnPrimaryAttackEnd.Invoke();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            pawn.weapon.OnSecondaryAttackBegin.Invoke();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            pawn.weapon.OnSecondaryAttackEnd.Invoke();
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
}




