using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class HealPackSpawner : SimulationBehaviour
{

    [SerializeField] GameObject healPackPrefab;
    float spawnTime = 0 ;
 
    public override void FixedUpdateNetwork()
    {
        if(spawnTime < Runner.SimulationTime)
        {
            Runner.Spawn(healPackPrefab, new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10)), Quaternion.identity);
            spawnTime = Runner.SimulationTime + 3;
        }
    }
}