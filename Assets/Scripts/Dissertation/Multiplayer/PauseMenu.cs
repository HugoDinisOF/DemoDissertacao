using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace Dissertation.Multiplayer {
    public class PauseMenu : NetworkBehaviour
    {
        public void QuitGame() {
            QuitGameServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void QuitGameServerRpc() {
            NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
