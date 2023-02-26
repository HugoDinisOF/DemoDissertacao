using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.DebugLoggers;
using Dissertation.Multiplayer;
using System;

namespace Dissertation.Core
{
    public class OnChangeImageTarget : MonoBehaviour
    {
        public static bool isImageTargetOn = false;

        public void OnTargetFound()
        {
            foreach (Rigidbody childRB in GetComponentsInChildren<Rigidbody>())
            {
                childRB.useGravity = true;
                // FIXME: Change this if we add rotation
                childRB.constraints = RigidbodyConstraints.FreezeRotation;
            }
            DebugStatics.detectTarget = "TRUE";
            isImageTargetOn = true;
            try
            {
                GameManager.instance.DebugServerRpc($"Target Found, {GameManager.instance.OwnerClientId}");
                DebugStatics.detectTarget = GameManager.instance.NetworkObjectId.ToString();
            }
            catch (Exception e)
            {
                DebugStatics.detectTarget = e.Message;
            }
            Debug.Log("TRUE");
        }
        public void OnTargetLost()
        {
            foreach (Rigidbody childRB in GetComponentsInChildren<Rigidbody>())
            {
                childRB.useGravity = false;
                childRB.constraints = RigidbodyConstraints.FreezeAll;
            }
            DebugStatics.detectTarget = "FALSE";
            isImageTargetOn = false;
            GameManager.instance.DebugServerRpc($"Target Lost, {GameManager.instance.OwnerClientId}");
            Debug.Log("FALSE");
        }

    }
}
