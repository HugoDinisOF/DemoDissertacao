using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.BlockLogic;

namespace Dissertation.Core
{
public class CheckBlockInside : MonoBehaviour
{
    public Block block;
    bool isInside = false;
    BoxCollider collider;
    BoxCollider overlappedCollider = null;
    float volume;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        volume = collider.bounds.size.x * collider.bounds.size.y * collider.bounds.size.z;
        Debug.Log(volume);
    }

    void FixedUpdate()
    {
        if (isInside)
        {
            float percentOverlap = CalculateOverlapPercent();
            if (percentOverlap > 60)
            {
                GameManager.instance.SetBlockState(block.id, true);
            }
            else
            {
                GameManager.instance.SetBlockState(block.id, false);
            }
        }
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
            if (block.IsCompatible(otherBlock))
            {
                // NOTE: Considering all our colliders will be box colliders for now :P
                overlappedCollider = (BoxCollider)other;
                isInside = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == overlappedCollider)
        {
            Debug.Log("LEAVING");
            isInside = false;
            GameManager.instance.SetBlockState(block.id, false);
            overlappedCollider = null;
        }
    }

    // Adapted from:
    //  https://stackoverflow.com/questions/71266589/overlapped-area-between-two-colliders
    public Vector3 OverlapArea(BoxCollider a, BoxCollider b)
    {
        // get the bounds of both colliders
        var boundsA = a.bounds;
        var boundsB = b.bounds;

        // first heck whether the two objects are even overlapping at all
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
