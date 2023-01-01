using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dissertation.Core { 
    public class WinScreenLogic : MonoBehaviour
    {

        public Button nextLevelBtn;
        public Button lobbyBtn;
        
        void Start()
        {
            nextLevelBtn.onClick.AddListener(LobbyManager.instance.LoadNextLevel);
            lobbyBtn.onClick.AddListener(LobbyManager.instance.LoadLobby);
        }
    }
}
