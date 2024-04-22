using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction : MonoBehaviour
{
    public virtual void Awake()
    {
    }
    public virtual void Start()
    {
    }
    public virtual void Update()
    {
        if(GameManager.instance != null)
        {
            if (GameManager.instance.isPaused) return;
        }
        

        //... the rest of the function goes here.
    }
}