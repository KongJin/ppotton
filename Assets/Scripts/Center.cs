using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class Center : SimulationBehaviour, INetworkRunnerCallbacks {

    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] Repoter repoter;
    [SerializeField] IPlayerInputSensor inputSensor ;
    // creating a instance of the Input Action created
    public void Awake()
    {   
        StartCoroutine(AddCallback());
    }

    public IEnumerator AddCallback()
    {
        while (Runner == null)
        {
            yield return null;
        }
        Debug.Log("connect");
        Runner.AddCallbacks(this);
    }

    PlayerInput myInput = new PlayerInput();
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (inputSensor == null) return;

        inputSensor.SetMoveDirection(ref myInput.Movement);
        inputSensor.SetButtonPressing(ref myInput.Buttons);

        input.Set(myInput);
    }

    public void OnDisable()
    {
        if (Runner != null)
        {
            Runner.RemoveCallbacks(this);
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log($"Center: OnObjectExitAOI player= {player}");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //Debug.Log($"OnObjectEnterAOI player= {player}");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerJoined player= {player}");
        UnityEngine.Random.InitState(1234);
        if (player == Runner.LocalPlayer)
        {
            var playerObject = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity, inputAuthority: Runner.LocalPlayer);
            repoter.Initialize(playerObject.transform);            
            inputSensor =  IPlayerInputSensor.GetInputSensor();
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Center: OnPlayerLeft player= {player}");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"Center: OnShutdown shutdownReason= {shutdownReason}");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log($"Center: OnDisconnectedFromServer NetDisconnectReason= {reason}");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log($"Center: OnConnectRequest request= {request}, token={token}");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)//2. startGame 실패했을때
    {
        Debug.Log($"Center: OnConnectFailed NetAddress= {remoteAddress} NetConnectFailedReason = {reason}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log($"Center: OnUserSimulationMessage SimulationMessagePtr= {message}");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log($"Center: OnReliableDataReceived player= {player} ReliableKey= {key} ArraySegment= {data}");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log($"Center: OnReliableDataProgress player= {player} ReliableKey= {key} progress= {progress}");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log($"Center: OnInputMissing player= {player} NetworkInput= {input}");
    }

    public void OnConnectedToServer(NetworkRunner runner) //1. StartGame이 호출되고 성공했을때
    {
        Debug.Log($"Center: OnConnectedToServer");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log($"Center: OnSessionListUpdated sessionList= {sessionList}");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log($"Center: OnCustomAuthenticationResponse Dictionary<string,object>= {data}");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log($"Center: OnHostMigration HostMigrationToken= {hostMigrationToken}");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log($"Center: OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log($"Center: OnSceneLoadStart");
    }
}