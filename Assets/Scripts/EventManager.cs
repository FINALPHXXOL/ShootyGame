using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatEvent : UnityEvent<float> 
{
    public UnityEvent OnDeath;
    public FloatEvent OnTakeDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPaused) return;

        //... the rest of the function goes here.
    }
}
