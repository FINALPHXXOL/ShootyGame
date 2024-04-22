using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponAction : GameAction
{
    public Weapon weapon;
    public GameActionPlaySound shotFiredSound;
    public GameActionSpawnParticles muzzleParticle;

    public override void Awake()
    {
        weapon = GetComponent<Weapon>();
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (GameManager.instance.isPaused) return;

        //... the rest of the function goes here.
        base.Update();
    }
}