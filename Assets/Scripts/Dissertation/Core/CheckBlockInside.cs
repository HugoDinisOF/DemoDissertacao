using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.BlockLogic;
using Dissertation.Multiplayer;
using Unity.Netcode;

namespace Dissertation.Core
{
    public class CheckBlockInside : AbstractNetworkObject
    {
        // variable to see if snap is active or not
        public static bool SNAP = true;

        public Block block;
        public float percentOverlapToCount = 50f;
        bool isInside = false;
        BoxCollider collider;
        BoxCollider overlappedCollider = null;
        LeanDragTranslateExt blockObject = null;
        float volume;
        float scaleFactor = 0.001f;

        override protected void Start()
        {
            base.Start();
            collider = GetComponent<BoxCollider>();
            volume = collider.bounds.size.x * collider.bounds.size.y * collider.bounds.size.z;
            Debug.Log(volume);
        }

        void FixedUpdate()
        {
            if (isInside && SNAP && NetworkManager.Singleton.IsServer)
            {
                Debug.Log("Checking Distance");
                if (Vector3.Distance(transform.position, blockObject.transform.position) < 0.002f)
                    return;
                Debug.Log("Checking isGrabbed & isMoving");
                if (blockObject.isGrabbed.Value || blockObject.isMoving.Value)
                    return;

                if (!CheckIfSomethingBelow(blockObject.transform))
                    return;

                Debug.Log($"Translating {Vector3.Lerp(blockObject.transform.position, transform.position, 0.3f) - blockObject.transform.position}"); 

                blockObject.transform.Translate(Vector3.Lerp(blockObject.transform.position, transform.position, 0.3f) - blockObject.transform.position);

                if (Vector3.Distance(transform.position, blockObject.transform.position) < 0.001f)
                    blockObject.transform.Translate(transform.position);

                /*
                float percentOverlap = CalculateOverlapPercent();
                Debug.Log(percentOverlap);
                if (percentOverlap > percentOverlapToCount)
                {
                    GameMode.instance.SetBlockState(block.id, true);
                }
                else
                {
                    GameMode.instance.SetBlockState(block.id, false);
                }
                */
            }
        }

        bool CheckIfSomethingBelow(Transform checkObject)
        {
            Vector3 centerPos = checkObject.position - new Vector3(0, checkObject.localScale.y / 2 + scaleFactor, 0);

            Collider[] colliders = Physics.OverlapBox(centerPos, new Vector3(scaleFactor/1.5f, scaleFactor, scaleFactor/1.5f));

            Debug.Log("CheckIfSomethingBelow: Start Check");
            foreach (Collider collider in colliders) 
            {
                Debug.Log($"CheckIfSomethingBelow: {collider.gameObject.name}");
                if (collider.gameObject.name.Contains("Holo") || collider.gameObject.name.Contains("ImageTarget") || collider.gameObject.name == checkObject.gameObject.name)
                {
                    continue;
                }
                Debug.Log($"CheckIfSomethingBelow: returning true");
                return true;
            }
            Debug.Log($"CheckIfSomethingBelow: returning false");
            return false;
        }

        float CalculateOverlapPercent()
        {
            Vector3 overlap = OverlapArea(collider, overlappedCollider);
            float overlapVolume = overlap.x * overlap.y * overlap.z;
            return overlapVolume / volume * 100f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Interactable") && overlappedCollider is null)
            {
                Block otherBlock = other.GetComponent<BlockHolder>().block;
                AbstractOwnershipAction ownershipAction = other.GetComponent<AbstractOwnershipAction>();
                if (block.IsCompatible(otherBlock))
                {
                    // NOTE: Considering all our colliders will be box colliders for now :P
                    overlappedCollider = (BoxCollider)other;
                    blockObject = other.GetComponent<LeanDragTranslateExt>();
                    isInside = true;
                    GameMode.instance.SetBlockState(block.id, true, ownershipAction.lastOwner);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == overlappedCollider)
            {
                Debug.Log($"LEAVING {block.id}");
                isInside = false;
                GameMode.instance.SetBlockState(block.id, false, 0);
                overlappedCollider = null;
                blockObject = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (blockObject != null) {
                Vector3 centerPos = blockObject.transform.position - new Vector3(0, blockObject.transform.localScale.y / 2 + scaleFactor, 0);
                Gizmos.DrawCube(centerPos, (new Vector3(scaleFactor / 1.5f, scaleFactor, scaleFactor / 1.5f)) * 2);
            }
        }

        // Adapted from:
        //  https://stackoverflow.com/questions/71266589/overlapped-area-between-two-colliders
        public Vector3 OverlapArea(BoxCollider a, BoxCollider b)
        {
            // get the bounds of both colliders
            var boundsA = a.bounds;
            var boundsB = b.bounds;

            // first check whether the two objects are even overlapping at all
            if (!boundsA.Intersects(boundsB))
            {
                return Vector3.zero;
            }

            // get min and max point of both
            var minA = boundsA.min; //(basically the bottom-left - back corner point)
            var maxA = boundsA.max; //(basically the top-right - front corner point)

            var minB = boundsB.min;
            var maxB = boundsB.max;

            // we want the smaller of the max and the higher of the min points
            var lowerMax = Vector3.Min(maxA, maxB);
            var higherMin = Vector3.Max(minA, minB);

            // the delta between those is now your overlapping area
            return lowerMax - higherMin;
        }
    }
}
