using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Dissertation.Multiplayer;
using System;
using Dissertation.DebugLoggers;
using TMPro;

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
            instance = null;
            base.OnDestroy();
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
            TextMeshProUGUI childTxt = null;
            foreach (var child in gameObjectWin.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (child.gameObject.name == "WinTxt")
                {
                    childTxt = child;
                    break;
                }
            }

            childTxt.text = "";
            for (int i = 0; i < LobbyManager.instance.playerStatsList.Count; i++)
            {
                var playerStats = LobbyManager.instance.playerStatsList[i];
                childTxt.text += $"{i}: Objects Grabbed: {playerStats.touches}, Objects placed in the right place: {playerStats.piecesPutInPlace}\n";
            }
        }

        public float GetCountedTime() { return countedTime.Value; }

    }
}
