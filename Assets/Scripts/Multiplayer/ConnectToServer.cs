using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
//using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviour
{
    public TMP_InputField inputField;

    public void SetConnectToServer() {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(inputField.text,7777);
        NetworkManager.Singleton.StartClient();
        enabled = false;
        //SceneManager.LoadScene(1);
    }
}
