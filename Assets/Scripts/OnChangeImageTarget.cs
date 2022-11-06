using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangeImageTarget : MonoBehaviour
{
    public void OnTargetFound() {
        foreach (Rigidbody childRB in GetComponentsInChildren<Rigidbody>()) 
        {
            childRB.useGravity = true;     
        }
        DebugStatics.detectTarget = "TRUE";
    }
    public void OnTargetLost() {
        foreach (Rigidbody childRB in GetComponentsInChildren<Rigidbody>())
        {
            childRB.useGravity = false;
        }
        DebugStatics.detectTarget = "FALSE";
    }

}
