using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
//using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviour
{
    public TMP_InputField inputField;

    public void SetConnectToServer() {
        try
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(inputField.text, 7777);
            if (NetworkManager.Singleton.StartClient())
            {
                enabled = false;
            }
            DebugStatics.debugObject = "fail connecting";
        } catch (Exception e)
        {
            DebugStatics.debugObject = e.Message;
        }
        //SceneManager.LoadScene(1);
    }
}
