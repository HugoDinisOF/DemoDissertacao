using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Dissertation.VR;
using UnityEngine.InputSystem;

namespace Dissertation.Core { 
    public class ArrowController : MonoBehaviour 
    {
        public static ulong id = 1;
        public Transform rightHand;
        public Transform leftHand;

        public GameObject arrowPile;
        // this one has colliders
        public GameObject spawnArrow;

        public InputActionReference LeftActivate; 
        public InputActionReference RightActivate;

        public void OnEnable()
        {
            LeftActivate.action.performed += (ctx) => { OnActivateLeft(); };
            RightActivate.action.performed += (ctx) => { OnActivateRight(); };

        }

        public void OnActivateRight() { OnActivate(true); }
        public void OnActivateLeft() { OnActivate(false); }

        public void OnActivate(bool isRight) {
            
            Transform arrowTransform = leftHand;
            if (isRight == true) {
                arrowTransform = rightHand;
            }

            GameObject arrow = Instantiate(spawnArrow, arrowTransform.position, arrowTransform.rotation);
            arrow.transform.parent = arrowPile.transform;
            arrow.GetComponent<VRArrow>().id = id;
            arrow.GetComponent<VRArrow>().localClientId = NetworkManager.Singleton.LocalClientId;

            GameManager.instance.SpawnNetworkArrowServerRpc(arrowTransform.position, arrowTransform.rotation, NetworkManager.Singleton.LocalClientId, id);
            id++;
        }

        // stupid I know
        public enum LeftRight {
            LEFT,
            RIGHT,
        }
        
    }
}
