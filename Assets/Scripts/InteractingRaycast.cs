using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingRaycast : MonoBehaviour
{
    public float minNearDistance = 0.09f;
    public float moveSensitivity = 1.2f;

    private Camera arCamera;
    private GameObject interactableObject;

    private Ray ray;
    private float distanceToObject;

    private Rigidbody interactableRb;
    private LeanDragTranslateExt interactableLd;

    private bool enableRaycast;

    void Start()
    {
        arCamera = GetComponent<Camera>();
        interactableObject = null;
        enableRaycast = true;
    }

    void Update()
    {
        ray = arCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (enableRaycast)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Interactable")))
            {
                Transform objectHit = hit.transform;

                distanceToObject = Vector3.Distance(hit.point, transform.position);
                if (distanceToObject < minNearDistance) {
                    distanceToObject = minNearDistance;
                }

                interactableObject = objectHit.gameObject;
                DebugStatics.debugObject = objectHit.gameObject.ToString();
                Debug.DrawRay(transform.position, ray.direction, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, ray.direction, Color.red);
                DebugStatics.debugObject = "Nothing";
                interactableObject = null;
            }
        }
        else 
        {
            // Move object towards the place it should be
            Vector3 translation = transform.position + ray.direction * distanceToObject - interactableObject.transform.position;
            interactableObject.transform.Translate(translation * moveSensitivity * Time.deltaTime);
        }
    }

    public void Interact() {
        if (enableRaycast)
            AttachObject();
        else
            DetachObject();
    }



    public void AttachObject()
    {
        if (interactableObject == null)
            return;

        interactableRb = interactableObject.GetComponent<Rigidbody>();
        interactableLd = interactableObject.GetComponent<LeanDragTranslateExt>();

        interactableRb.useGravity = false;
        //rb.isKinematic = true;
        interactableRb.mass = 5;

        interactableLd.SetIsGrabbed(true);
        enableRaycast = false;

        Debug.Log("Attach");
    }

    public void DetachObject()
    {

        interactableRb.useGravity = OnChangeImageTarget.isImageTargetOn;
        //rb.isKinematic = false;
        interactableRb.mass = 1;

        interactableLd.SetIsGrabbed(false);
        enableRaycast = true;
        Debug.Log("Detach");
    }

}
