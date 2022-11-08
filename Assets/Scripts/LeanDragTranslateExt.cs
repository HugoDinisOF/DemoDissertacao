using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

[RequireComponent(typeof(Rigidbody))]
public class LeanDragTranslateExt : LeanDragTranslate
{

    Rigidbody rb;
    bool isGrabbed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    override protected void Update()
    {
        if (OnChangeImageTarget.isImageTargetOn & !isGrabbed)
        {
            base.Update();
            var fingers = Use.UpdateAndGetFingers();
            rb.useGravity = fingers.Count == 0;
            if (fingers.Count >= 1)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else {
            // have to call UpdateAndGetFingers() every frame as noted in the same function notes
            var fingers = Use.UpdateAndGetFingers();
        }
    }

    public void SetIsGrabbed(bool state) {
        isGrabbed = state;
    }
}
