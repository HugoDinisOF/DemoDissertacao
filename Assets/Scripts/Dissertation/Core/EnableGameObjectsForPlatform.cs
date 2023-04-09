using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsForPlatform : MonoBehaviour
{
    public List<GameObject> mobileGameObjects = new List<GameObject>();
    public List<GameObject> vrGameObjects = new List<GameObject>();
    public List<GameObject> serverGameObjects = new List<GameObject>();

    void Start()
    {
        switch (PlatformStatics.platform) {
            case PlatformStatics.BuildPlatform.SERVER:
                DisableAllGameObjects(vrGameObjects);
                DisableAllGameObjects(mobileGameObjects);

                EnableAllGameObjects(serverGameObjects);
                break;
            case PlatformStatics.BuildPlatform.MOBILE:
                DisableAllGameObjects(vrGameObjects);
                DisableAllGameObjects(serverGameObjects);

                EnableAllGameObjects(mobileGameObjects);
                break;
            case PlatformStatics.BuildPlatform.VR:
                DisableAllGameObjects(mobileGameObjects);
                DisableAllGameObjects(serverGameObjects);

                EnableAllGameObjects(vrGameObjects);
                break;
            default:
                break;

        }
    }

    void EnableAllGameObjects(List<GameObject> gameObjects) 
    {
        foreach (GameObject gameObject in gameObjects) {
            gameObject.SetActive(true);
        }
    }

    void DisableAllGameObjects(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
    }


}
