using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dissertation.DebugLoggers
{
    public class GrabReleaseTextBtn : MonoBehaviour
    {
        TextMeshProUGUI textMesh;

        private void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (textMesh.text != DebugStatics.grabReleaseBtnText)
                textMesh.text = DebugStatics.grabReleaseBtnText;
        }
    } 
}
