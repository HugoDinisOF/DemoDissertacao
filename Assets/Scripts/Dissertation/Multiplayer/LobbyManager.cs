using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Dissertation.Multiplayer { 

    public class LobbyManager : AbstractNetworkObject
    {
        static ulong NOHOSTID = 0xFFFF;

        public NetworkVariable<ulong> hostClientId = new NetworkVariable<ulong>(NOHOSTID);
        public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);
        
        protected override void Start()
        {
            base.Start();
            if (NetworkManager.Singleton.IsServer) 
            { 
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
            }
        }

        private void Singleton_OnClientConnectedCallback(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (hostClientId.Value == NOHOSTID)
                {
                    hostClientId.Value = clientId;
                }
            }
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer) { 
                // other logic may be added here like UI;
                
                // when `host` disconnects
                if(hostClientId.Value == clientId) 
                {
                    hostClientId.Value = NOHOSTID;
                    SelectNextHost();
                }
            }
        }

        void Update()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (hostClientId.Value == NOHOSTID && NetworkManager.Singleton.ConnectedClientsIds.Count > 0)
                {
                    Debug.Log(NetworkManager.Singleton.ConnectedClientsList.Count);
                    hostClientId.Value = NetworkManager.Singleton.ConnectedClientsIds[0];
                }   
            }
        }

        private void SelectNextHost() {
            if (hostClientId.Value == NOHOSTID && NetworkManager.Singleton.ConnectedClientsIds.Count > 0)
            {
                Debug.Log(NetworkManager.Singleton.ConnectedClientsList.Count);
                hostClientId.Value = NetworkManager.Singleton.ConnectedClientsIds[0];
            }
        }


        public void LoadNextScene(string sceneName) 
        {
            LoadSceneServerRpc(sceneName);
            if (NetworkManager.Singleton.IsServer)
                NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        [ServerRpc(RequireOwnership = false)]
        public void LoadSceneServerRpc(string sceneName) {
            NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

    }
}
