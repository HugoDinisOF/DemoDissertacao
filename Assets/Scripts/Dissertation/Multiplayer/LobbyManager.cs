using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Dissertation.Multiplayer { 

    public class LobbyManager : AbstractNetworkObject
    {
        
        protected override void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log(NetworkManager.Singleton.ConnectedClientsList.Count);
            }
             
        }
    }
}
