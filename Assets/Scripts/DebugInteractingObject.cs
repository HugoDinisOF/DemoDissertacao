using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInteractingObject : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        textMeshPro.text = DebugStatics.debugObject; 
    }
}
