using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance = null;
    public Dictionary<int,bool> blocksDone;

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
        Debug.Log("GAME WIN");
    }

    
}
