using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.Multiplayer;
using Unity.Netcode;

namespace Dissertation.Core 
{ 
    public class GameMode : AbstractNetworkObject
    {
        public static GameMode instance;

        public GameModeRules gameModeRules;
        public List<Transform> spawnLocations;
        public GameObject target;
        public int score;
        public bool isWinning;


        private NetworkVariable<float> gameTimeLeft = new NetworkVariable<float>(-1);
        private float timeToWinLeft = -1f;

        override protected void Start()
        {
            if (!(instance is null))
            {
                Destroy(this);
                return;
            }
            instance = this;
            AbstractOwnershipAction.isAllowedToChangeOwnership = gameModeRules.ownershipType == GameModeRules.OwnershipType.EVERYONE;

            if (!NetworkManager.Singleton.IsServer) return;

            // NOTE: base.Start is not called here because the GameManager should run it before  
            gameTimeLeft.Value = gameModeRules.gameTime;
            SpawnPieces();
        }

        void Update()
        {
            if (!NetworkManager.Singleton.IsServer) return;

            gameTimeLeft.Value -= Time.deltaTime;

            if (isWinning) 
            {
                timeToWinLeft -= Time.deltaTime;    
            }

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            instance = null;
        }

        void SpawnPieces() {
            int spawnloc = 0;
            foreach (var pieceTypeCount in gameModeRules.gamePieces) {
                for(int i = 0; i < pieceTypeCount.count; i++) { 
                    // TODO: Add change of height when all the spots are filled
                    GameObject piece = Instantiate(pieceTypeCount.pieceObject, target.transform);
                    piece.transform.position = spawnLocations[spawnloc % spawnLocations.Count].position;
                    NetworkObject networkObject = piece.GetComponent<NetworkObject>();
                    networkObject.Spawn();
                    networkObject.TrySetParent(target);
                    if (gameModeRules.ownershipType != GameModeRules.OwnershipType.EVERYONE) {
                        networkObject.ChangeOwnership(LobbyManager.instance.playerList[pieceTypeCount.playerId]);
                    }
                    spawnloc++;
                }
            }
        }
    }
}
