using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

[RequireComponent(typeof(Rigidbody))]
public class LeanDragTranslateExt : LeanDragTranslate
{

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    override protected void Update()
    {
        base.Update();
        if (OnChangeImageTarget.isImageTargetOn)
        {
            var fingers = Use.UpdateAndGetFingers();
            rb.useGravity = fingers.Count == 0;
            if (fingers.Count >= 1)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
