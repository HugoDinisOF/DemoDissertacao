using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.Multiplayer;
using Unity.Netcode;
using Dissertation.DebugLoggers;

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
        public Dictionary<int, bool> blocksDone;
        public NetworkVariable<float> gameTimeLeft = new NetworkVariable<float>(-1);
        public NetworkVariable<float> gameTimePassed = new NetworkVariable<float>(0f);


        public float timeToWinLeft = -1f;


        override protected void Start()
        {
            if (!(instance is null))
            {
                Destroy(this);
                return;
            }
            instance = this;
            AbstractOwnershipAction.isAllowedToChangeOwnership = gameModeRules.ownershipType == GameModeRules.OwnershipType.EVERYONE;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;


            if (!NetworkManager.Singleton.IsServer) return;

            // NOTE: base.Start is not called here because the GameManager should run it before  
            gameTimeLeft.Value = gameModeRules.gameTime;
            timeToWinLeft = gameModeRules.timeToWin;

            //NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            //SpawnPieces();
            GetAndSetBlocksDone();
        }

        private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            Debug.Log("clients complete");
            foreach (var client in clientsCompleted)
            {
                Debug.Log(client);
            }
            Debug.Log("clients timed out");
            foreach (var client in clientsTimedOut)
            {
                Debug.Log(client);
            }
            if (!NetworkManager.Singleton.IsServer) return;
            SpawnPieces();
        }

        void Update()
        {
            DebugStatics.detectTarget = Mathf.Round(gameTimePassed.Value).ToString();

            if (!NetworkManager.Singleton.IsServer) return;

            gameTimeLeft.Value -= Time.deltaTime;
            gameTimePassed.Value += Time.deltaTime;

            if (isWinning) 
            {
                timeToWinLeft -= Time.deltaTime;

                if (timeToWinLeft < 0)
                {
                    GameManager.instance.WinGameClientRpc();
                    Debug.Log("win!");
                    // FIXME: Maybe not the best solution since some magic may happen and the 
                    isWinning = false;
                }
            }

        }

        public override void OnDestroy()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
            instance = null;
            base.OnDestroy();
        }

        void GetAndSetBlocksDone()
        {
            blocksDone = new Dictionary<int, bool>();
            foreach (CheckBlockInside child in target.GetComponentsInChildren<CheckBlockInside>())
            {
                Debug.Log($"Child: {child.gameObject.name}");
                blocksDone.Add(child.block.id, false);
            }
        }

        public void SetBlockState(int id, bool value, ulong lastOwnerId)
        {
            if (!NetworkManager.Singleton.IsServer) return;

            Debug.Log($"{id} {value} {blocksDone[id]}");

            // Don't do anything if value is the same;
            if (!(blocksDone[id] ^ value))
                return;

            blocksDone[id] = value;
            if (value)
                LobbyManager.instance.AddPieceInPlace(lastOwnerId);
            else
                LobbyManager.instance.RemovePieceInPlace(lastOwnerId);
            foreach (int key in blocksDone.Keys)
            {
                if (!blocksDone[key])
                {
                    isWinning = false;
                    timeToWinLeft = gameModeRules.timeToWin;
                    return;
                }
            }
            isWinning = true;
            Debug.Log("Is about to win");
            //WinGame();
        }

        void SpawnPieces() {
            int spawnloc = 0;
            foreach (var pieceTypeCount in gameModeRules.gamePieces) {
                for(int i = 0; i < pieceTypeCount.count; i++) { 
                    GameObject piece = Instantiate(pieceTypeCount.pieceObject, target.transform);
                    piece.transform.position = spawnLocations[spawnloc % spawnLocations.Count].position + Mathf.Floor(spawnloc / spawnLocations.Count) * new Vector3(0,0.0221f,0);
                    NetworkObject networkObject = piece.GetComponent<NetworkObject>();
                    networkObject.Spawn();
                    networkObject.TrySetParent(target);
                    if (gameModeRules.ownershipType != GameModeRules.OwnershipType.EVERYONE) {
                        AbstractOwnershipAction ownershipAction = piece.GetComponent<AbstractOwnershipAction>();
                        ownershipAction.ownerID.Value = pieceTypeCount.playerId;
                        if (pieceTypeCount.playerId != -1)
                        {
                            ownershipAction.ownerID.Value = (int) LobbyManager.instance.playerList[pieceTypeCount.playerId];
                            //networkObject.ChangeOwnership(LobbyManager.instance.playerList[pieceTypeCount.playerId]);
                        }
                    }
                    spawnloc++;
                }
            }
        }
    }
}
