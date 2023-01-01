using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Dissertation.Multiplayer;

namespace Dissertation.Core { 

    public class LobbyManager : AbstractNetworkObject
    {
        // FIXME: When we come back to 'LobbyScene' fix buttons not having the onclick to here! 


        static ulong NOHOSTID = 0xFFFF;

        // NOTE: Maybe change this stuff to a separate static class
        static string lobbySceneName = "LobbyScene";

        // FIXME: This can't stay like this! please find a way to save a string!
        public static Dictionary<string, ulong> levelNameDict = new Dictionary<string, ulong>()
        {
            {"MultiplayerScene", 0},
            {"LobbyScene", 99999}
        };

        public static Dictionary<ulong, string> nextLevelDict = new Dictionary<ulong, string>()
        {
            {0, "MultiplayerScene"}
        };

        public static LobbyManager instance = null;

        public NetworkVariable<ulong> hostClientId = new NetworkVariable<ulong>(NOHOSTID);
        // FIXME: Don't let this stay as a ulong
        public NetworkVariable<ulong> currentScene = new NetworkVariable<ulong>(0);
        public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);
        public NetworkList<ulong> playerList = new NetworkList<ulong>();


        protected override void Start()
        {
            base.Start();
            if (!(instance is null))
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;

            if (NetworkManager.Singleton.IsServer) 
            { 
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
            }

            // NOTE: Don't destroy on Load has to be added in script or it doesn't work!!!
            DontDestroyOnLoad(this.gameObject);
        }

        private void Singleton_OnClientConnectedCallback(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                playerList.Add(clientId);
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

                // TODO: remove user from the list 
                
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
            // FIXME: Doesn't change currentScene if the server changes scene
            LoadSceneServerRpc(sceneName);
            if (NetworkManager.Singleton.IsServer)
                NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        [ServerRpc(RequireOwnership = false)]
        public void LoadSceneServerRpc(string sceneName) {
            currentScene.Value = levelNameDict[sceneName];
            NetworkManager.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void LoadNextLevel() {
            LoadNextScene(nextLevelDict[currentScene.Value]);
        }

        public void LoadLobby() {
            LoadNextScene(lobbySceneName);
        }

    }
}