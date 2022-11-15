using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    
    public static GameManager instance = null;
    private Dictionary<int,bool> blocksDone;
    public GameObject gameObjectWin;
    public Button grabBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (!(instance is null)) {
            Destroy(this);
            return;
        }
        instance = this;
        GetAndSetBlocksDone();
    }

    void GetAndSetBlocksDone() {
        blocksDone = new Dictionary<int, bool>();
        GameObject parent = GameObject.Find("ImageTarget");
        foreach (CheckBlockInside child in parent.GetComponentsInChildren<CheckBlockInside>()) {
            blocksDone.Add(child.block.id, false);
        }
    }

    public void SetBlockState(int id, bool value) {
        // Don't do anything if value is the same;
        if (!(blocksDone[id] ^ value))
            return;

        blocksDone[id] = value;
        foreach (int key in blocksDone.Keys) {
            if (!blocksDone[key]) {
                return;
            }
        }
        WinGame();
    }

    void WinGame()
    {
        gameObjectWin.SetActive(true);
        Debug.Log("GAME WIN");
    }

}
