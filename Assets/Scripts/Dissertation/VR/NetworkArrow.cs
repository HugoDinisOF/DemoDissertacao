using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.Multiplayer;
using Unity.Netcode;

namespace Dissertation.VR { 
    public class NetworkArrow : AbstractNetworkObject
    {
        public NetworkVariable<ulong> id = new NetworkVariable<ulong>();
        public NetworkVariable<ulong> localClientId = new NetworkVariable<ulong>();

        public GameObject arrow = null;

        private void Start()
        {
            if (localClientId.Value != NetworkManager.Singleton.LocalClientId) { 
                enabled = false;
                return;
            }

            FindArrow();
        }

        private void Update()
        {
            if (arrow is null) {
                FindArrow();
                return;
            }

            transform.position = arrow.transform.position;
            transform.rotation = arrow.transform.rotation;
        }

        private void FindArrow() {
            GameObject pile = GameObject.Find("ArrowPile");

            foreach (VRArrow go in pile.GetComponentsInChildren<VRArrow>())
            {
                if (go.id == id.Value)
                {
                    arrow = go.gameObject;
                    break;
                }
            }
            Debug.Log($"Arrow with id {id.Value} not found on client {localClientId.Value}");
        }
    }
}
