using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Dissertation.Multiplayer;

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
            if (!(instance is null))
            {
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
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            instance = null;
        }

        [ClientRpc]
        public void WinGameClientRpc()
        {
            gameObjectWin.SetActive(true);
            Debug.Log("GAME WIN");
        }

        public float GetCountedTime() { return countedTime.Value; }

    }
}
