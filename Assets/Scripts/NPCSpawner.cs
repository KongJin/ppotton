using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.KCC;
using Fusion.Sockets;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] GameObject npc;

    public void MakeNpcs(NetworkRunner runner)
    {
        for (int i = 0; i < 50; ++i)
        {
            runner.Spawn(npc, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
        }
    }

}