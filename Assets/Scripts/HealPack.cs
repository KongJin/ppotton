using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class HealPack : NetworkObject
{    

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out PlayerHealth health);
        {
            if (health == null) return;
            health.HealRpc(30);
            Runner.Despawn(this);
        }
    }
}
