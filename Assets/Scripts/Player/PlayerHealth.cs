using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    private Material _body;

    private void Awake()
    {
        _body = GetComponent<Material>();
        DontDestroyOnLoad(this);
    }


    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHealth { get; set; } = 30;

    private void HealthChanged()//UI
    {
        Debug.Log($"OnChangedRender 업데이트할게. {NetworkedHealth}");

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    public void DealDamageRpc(float damage)
    {
        NetworkedHealth -= damage;
        NetworkedHealth = Mathf.Clamp(NetworkedHealth, 0, 100);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    public void HealRpc(float value)
    {
        NetworkedHealth += value;
        NetworkedHealth = Mathf.Clamp(NetworkedHealth, 0, 100);
    }

}
