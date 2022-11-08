using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingRaycast : MonoBehaviour
{
    public float minNearDistance = 0.09f;
    public float moveSensitivity = 1.2f;

    private Camera arCamera;
    private GameObject interactableObject;
    private GameObject fatherInteractable;
    private Vector3 fixedPosition;
    private Vector3 originalRotation;

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
            //Vector3 rotate = transform.rotation.eulerAngles - lastRotation;
            Vector3 translation = transform.position + ray.direction * distanceToObject - interactableObject.transform.position;
            //Vector3 translation = fixedPosition - interactableObject.transform.localPosition;
            interactableObject.transform.Translate(translation * moveSensitivity * Time.deltaTime);

            
            //interactableObject.transform.RotateAround(interactableObject.transform.position, Vector3.down, originalRotation.x);
            //interactableObject.transform.RotateAround(interactableObject.transform.position, Vector3.right, originalRotation.y);
            //interactableObject.transform.RotateAround(interactableObject.transform.position, Vector3.forward, originalRotation.z);
            //lastRotation = transform.rotation.eulerAngles;
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
        //interactableObject.transform.parent = transform;

        interactableRb = interactableObject.GetComponent<Rigidbody>();
        interactableLd = interactableObject.GetComponent<LeanDragTranslateExt>();

        interactableRb.useGravity = false;
        //rb.isKinematic = true;
        interactableRb.mass = 5;

        interactableLd.SetIsGrabbed(true);
        enableRaycast = false;
        fixedPosition = interactableObject.transform.localPosition;
        originalRotation = transform.rotation.eulerAngles;
        //interactableObject.transform.LookAt(transform.position);
        //lastRotation = transform.rotation.eulerAngles;
        Debug.Log("Attach");
    }

    public void DetachObject() {
        interactableObject.transform.parent = fatherInteractable.transform;

        interactableRb.useGravity = OnChangeImageTarget.isImageTargetOn;
        //rb.isKinematic = false;
        interactableRb.mass = 1;

        interactableLd.SetIsGrabbed(false);
        enableRaycast = true;
        Debug.Log("Detach");
    }

}
