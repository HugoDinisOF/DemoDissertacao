using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OnTouchBehaviours : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnFingerDown() {
        rb.useGravity = false;
    }

    public void OnFingerUp() {
        rb.useGravity = true;
    }
}
