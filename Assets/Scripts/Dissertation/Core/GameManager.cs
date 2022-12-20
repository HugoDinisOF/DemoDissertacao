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

        private Dictionary<int, bool> blocksDone;
        private bool isWinning;
        private NetworkVariable<float> countedTime = new NetworkVariable<float>(0f);

        // Start is called before the first frame update
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

            if (NetworkManager.Singleton.IsServer) {
                Debug.Log("run this");
                GetAndSetBlocksDone();
                countedTime.Value = 0f;
            }
        }

        void Update()
        {
            if (NetworkManager.Singleton.IsServer && isWinning) {
                countedTime.Value += Time.deltaTime;
                if (countedTime.Value >= timeToWin) {
                    WinGameClientRpc();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            instance = null;
        }

        void GetAndSetBlocksDone()
        {
            blocksDone = new Dictionary<int, bool>();
            GameObject parent = GameObject.Find("ImageTarget");
            foreach (CheckBlockInside child in parent.GetComponentsInChildren<CheckBlockInside>())
            {
                Debug.Log($"Child: {child.gameObject.name}");
                blocksDone.Add(child.block.id, false);
            }
        }

        public void SetBlockState(int id, bool value)
        {
            if (!NetworkManager.Singleton.IsServer) return;

            Debug.Log($"{id} {value}");
            Debug.Log($"{blocksDone[id]}");

            // Don't do anything if value is the same;
            if (!(blocksDone[id] ^ value))
                return;

            blocksDone[id] = value;
            foreach (int key in blocksDone.Keys)
            {
                if (!blocksDone[key])
                {
                    isWinning = false;
                    countedTime.Value = 0f;
                    return;
                }
            }
            isWinning = true;
            Debug.Log("Is about to win");
            //WinGame();
        }

        [ClientRpc]
        void WinGameClientRpc()
        {
            gameObjectWin.SetActive(true);
            Debug.Log("GAME WIN");
        }

        public float GetCountedTime() { return countedTime.Value; }

    }
}
