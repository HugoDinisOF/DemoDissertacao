using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.Multiplayer;
using Unity.Netcode;

namespace Dissertation.Core 
{ 
    public class GameMode : AbstractNetworkObject
    {
        public GameModeRules gameModeRules;
        public Transform spawnLocation;
        public GameObject target;
        public int score;
        public bool isWinning;


        private NetworkVariable<float> gameTimeLeft = new NetworkVariable<float>(-1);
        private float timeToWinLeft = -1f;

        override protected void Start()
        {
            if (!NetworkManager.Singleton.IsServer) return;

            // NOTE: base.Start is not called here because the GameManager should run it before  
            gameTimeLeft.Value = gameModeRules.gameTime;
            SpawnPieces();

        }

        // Update is called once per frame
        void Update()
        {
            if (!NetworkManager.Singleton.IsServer) return;

            gameTimeLeft.Value -= Time.deltaTime;

            if (isWinning) 
            {
                timeToWinLeft -= Time.deltaTime;    
            }

        }

        void SpawnPieces() {
            foreach (var pieceTypeCount in gameModeRules.gamePieces) {
                for(int i = 0; i < pieceTypeCount.count; i++) { 
                    GameObject piece = Instantiate(pieceTypeCount.pieceObject, target.transform);
                    NetworkObject networkObject = piece.GetComponent<NetworkObject>();
                    networkObject.Spawn();
                    if (gameModeRules.ownershipType != GameModeRules.OwnershipType.EVERYONE) { 
                        //TODO: Change Ownership to the right player in the playerId var
                    }
                }
            }
        }
    }
}
