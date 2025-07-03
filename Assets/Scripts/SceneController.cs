using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class SceneController : NetworkObject
{
    //[Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    public void RPC_GotoBattle()
    {
        
        Runner.LoadScene(SceneRef.FromIndex(1), new LoadSceneParameters(LoadSceneMode.Single), setActiveOnLoad: true);
    }
 
}
