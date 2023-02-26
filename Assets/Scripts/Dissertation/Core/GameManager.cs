using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Dissertation.Multiplayer;
using System;
using Dissertation.DebugLoggers;

namespace Dissertation.Core
{
    public class GameManager : AbstractNetworkObject
    {

        public static GameManager instance = null;
        public GameObject gameObjectWin;
        public Button grabBtn;
        public GameObject MainPlayerCamera;
        // Time to win in seconds
        public float timeToWin = 5f;

        private NetworkVariable<float> countedTime = new NetworkVariable<float>(0f);

        override protected void Start()
        {
            base.Start();
            LobbyManager.instance.DebugServerRpc($"{NetworkManager.Singleton.LocalClientId} - {gameObjectWin.name}-{grabBtn.name}-{MainPlayerCamera.name}-{GetHashCode()}-{NetworkObjectId}");
            if (!(instance is null))
            {
                LobbyManager.instance.DebugServerRpc($"{NetworkManager.Singleton.LocalClientId} - Destroyed {NetworkObjectId}");
                Destroy(this);
                return;
            }
            instance = this;

            Debug.Log($"Is Server: {NetworkManager.Singleton.IsServer}");
        }

        void Update()
        {
            if (NetworkManager.Singleton.IsServer) {
            }
            DebugStatics.detectTarget = NetworkObjectId.ToString() + " " + LobbyManager.instance.NetworkObjectId.ToString();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            instance = null;
        }

        [ClientRpc]
        public void WinGameClientRpc()
        {
            DebugServerRpc("Before Win");
            if (gameObjectWin is null)
            {
                DebugServerRpc("Win is null");
            }
            gameObjectWin.SetActive(true);
            DebugServerRpc("After Win");
            Debug.Log("GAME WIN");

        }

        public float GetCountedTime() { return countedTime.Value; }

    }
}
