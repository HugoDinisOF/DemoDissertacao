using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.DebugLoggers;
using Dissertation.Multiplayer;

namespace Dissertation.Core
{
    public class OnChangeImageTarget : AbstractOwnershipAction
    {
        public static bool isImageTargetOn = false;

        public void OnTargetFound()
        {
            foreach (Rigidbody childRB in GetComponentsInChildren<Rigidbody>())
            {
                childRB.useGravity = true;
            }
            DebugStatics.detectTarget = "TRUE";
            isImageTargetOn = true;
            Debug.Log("TRUE");
        }
        public void OnTargetLost()
        {
            foreach (Rigidbody childRB in GetComponentsInChildren<Rigidbody>())
            {
                childRB.useGravity = false;
            }
            DebugStatics.detectTarget = "FALSE";
            isImageTargetOn = false;
            Debug.Log("FALSE");
        }

    }
}
