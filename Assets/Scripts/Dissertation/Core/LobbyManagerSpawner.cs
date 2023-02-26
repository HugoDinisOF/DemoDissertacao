using UnityEngine;
using Dissertation.Multiplayer;
using Unity.Netcode;

namespace Dissertation.Core { 

    public class LobbyManagerSpawner : AbstractNetworkObject
    {
        public GameObject lobbyManager;

        protected override void Start()
        {
            base.Start();
            if (NetworkManager.Singleton.IsServer && LobbyManager.instance is null)
            {
                GameObject lobby = Instantiate(lobbyManager);
                NetworkObject networkObject = lobby.GetComponent<NetworkObject>();
                networkObject.Spawn();
            }
        }
    }
}
