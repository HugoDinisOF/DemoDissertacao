using System.Collections.Generic;
using UnityEngine;
using Dissertation.Multiplayer;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

namespace Dissertation.Core { 

    public class LobbyManagerSpawner : AbstractNetworkObject
    {
        public GameObject lobbyManager;
        public GameObject buttonPrefab;
        public List<GameObject> buttonLayoutList;

        protected override void Start()
        {
            base.Start();
            if (NetworkManager.Singleton.IsServer && LobbyManager.instance is null)
            {
                GameObject lobby = Instantiate(lobbyManager);
                NetworkObject networkObject = lobby.GetComponent<NetworkObject>();
                networkObject.Spawn();
            }
            foreach (GameObject buttonLayout in buttonLayoutList) {
                SpawnButtons(buttonLayout);
            }
        }
        void SpawnButtons(GameObject buttonLayout) {
            foreach (string levelName in LobbyManager.levelNameToScene.Keys)
            {
                GameObject buttonObject = Instantiate(buttonPrefab, buttonLayout.transform);
                Button button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(delegate { LobbyManager.instance.LoadNextScene(LobbyManager.levelNameToScene[levelName]); });
                TextMeshProUGUI textmesh = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
                textmesh.text = levelName;
            }
        }

    }
}
