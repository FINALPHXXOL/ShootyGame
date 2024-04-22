using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : SpawnPoint
{
    public void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.playerSpawns.Add(this);
        }
    }

    public void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.playerSpawns.Remove(this);
        }
    }
}
