using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingRaycast : MonoBehaviour
{
    private Camera arCamera;
    private GameObject interactableObject;
    private GameObject fatherInteractable;
    private bool enableRaycast;

    void Start()
    {
        arCamera = GetComponent<Camera>();
        interactableObject = null;
        enableRaycast = true;
    }

    void Update()
    {
        if (enableRaycast) { 
            RaycastHit hit;
            Ray ray = arCamera.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));


            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Interactable")))
            {
                Transform objectHit = hit.transform;
                interactableObject = objectHit.gameObject;
                DebugStatics.debugObject = objectHit.gameObject.ToString();
                Debug.DrawRay(transform.position, ray.direction, Color.green);
            }
            else { 
                Debug.DrawRay(transform.position, ray.direction, Color.red);
                DebugStatics.debugObject = "Nothing";
                interactableObject = null;
            }
        }
    }

    public void Interact() {
        if (enableRaycast)
            AttachObject();
        else
            DetachObject();
    }



    public void AttachObject() {
        if (interactableObject == null) {
            return;
        }
        fatherInteractable = interactableObject.transform.parent.gameObject;
        interactableObject.transform.parent = transform;
        interactableObject.GetComponent<Rigidbody>().useGravity = false;
        enableRaycast = false;
        Debug.Log("Attach");
    }

    public void DetachObject() {
        interactableObject.transform.parent = fatherInteractable.transform;
        interactableObject.GetComponent<Rigidbody>().useGravity = true;
        enableRaycast = true;
        Debug.Log("Detach");
    }

}
