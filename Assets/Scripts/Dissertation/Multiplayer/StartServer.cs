using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class StartServer : MonoBehaviour
{
    public void TryStartServer() {
        NetworkManager.Singleton.StartServer();
        SceneManager.LoadScene(1);
    }
}
