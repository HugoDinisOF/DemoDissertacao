using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Dissertation.DebugLoggers;
using Dissertation.Multiplayer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Dissertation.Core
{
    public class InteractingRaycast : AbstractOwnershipAction
    {
        public float minNearDistance = 0.09f;
        public float moveSensitivity = 1.2f;
        public bool isOnline = true;
        public LineRenderer lineRenderer;

        private Camera arCamera;
        private GameObject interactableObject;

        private Ray ray;
        private float distanceToObject;

        private Rigidbody interactableRb;
        private LeanDragTranslateExt interactableLd;
        private AbstractOwnershipAction networkInteractable;

        private bool enableRaycast;

        override protected void Start()
        {
            base.Start();
            NetworkManager.SceneManager.OnLoadComplete += OnChangeScene;

            if (!IsOwner) return;

            // tries to get the component if it fails it is in (presumably) MP mode so grab the MainPlayerCamera
            OnChangeScene(0);
            interactableObject = null;
            enableRaycast = true;
        }

        void Update()
        {
            if (GameManager.instance is null) return;

            //lineRenderer.SetPosition(0, transform.position);
            //lineRenderer.SetPosition(1, transform.position + transform.forward * 0.3f);
            //lineRenderer.startWidth = 0.005f;
            //lineRenderer.endWidth = 0.005f;  

            if (!IsOwner & isOnline) return;

            // Update the position of the player as they move their camera 
            transform.position = GameManager.instance.MainPlayerCamera.transform.position;
            transform.rotation = GameManager.instance.MainPlayerCamera.transform.rotation;

            ray = arCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (enableRaycast)
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 25f, LayerMask.GetMask("Interactable")))
                {
                    Transform objectHit = hit.transform;

                    distanceToObject = Vector3.Distance(hit.point, transform.position);
                    if (distanceToObject < minNearDistance)
                    {
                        distanceToObject = minNearDistance;
                    }

                    interactableObject = objectHit.gameObject;
                    DebugStatics.debugObject = objectHit.gameObject.name;
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
                // Move object towards the place it should be if it is the owner
                if (networkInteractable.IsOwner && !networkInteractable.isRemoving)
                {
                    Vector3 translation = transform.position + ray.direction * distanceToObject - interactableObject.transform.position;
                    interactableObject.transform.Translate(translation * moveSensitivity * Time.deltaTime);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (GameManager.instance is null) return;
            
            // FIXME: Ternary Operator
            if (IsOwner) {
                Gizmos.color = Color.white;
            } 
            else 
            {
                Gizmos.color = Color.blue;
            }

            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 100f);
        }

        public void Interact()
        {
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
            DebugServerRpc("PreAttach");

            networkInteractable = interactableObject.GetComponent<AbstractOwnershipAction>();
            if (networkInteractable.ChangeOwnership() || networkInteractable.IsOwner)
            {
                enableRaycast = false;
                DebugStatics.grabReleaseBtnText = DebugStatics.releaseText;
            }

            DebugServerRpc("Attach");
            Debug.Log("Attach");
        }

        public void DetachObject()
        {

            interactableRb.useGravity = OnChangeImageTarget.isImageTargetOn;
            //rb.isKinematic = false;
            interactableRb.mass = 1;

            interactableLd.SetIsGrabbed(false);
            enableRaycast = true;

            networkInteractable.RemoveOwnership();
            DebugStatics.grabReleaseBtnText = DebugStatics.grabText;

            DebugServerRpc("Detach");
            Debug.Log("Detach");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log("CONNECTED TO SERVER");
        }

        public void OnChangeScene(ulong clientId, string sceneName="", LoadSceneMode loadSceneMode=LoadSceneMode.Single) {
            Debug.Log($"{clientId} - ChangeScene");
            if (!IsOwner) return;
            
            if (!TryGetComponent<Camera>(out arCamera))
            {
                arCamera = Camera.main;
                Debug.Log($"GameManager.instance -> {GameManager.instance}");
                if (GameManager.instance != null)
                {
                    GameManager.instance.grabBtn.onClick.AddListener(Interact);
                    transform.position = GameManager.instance.MainPlayerCamera.transform.position;
                    transform.rotation = GameManager.instance.MainPlayerCamera.transform.rotation;
                }
                else {
                    StartCoroutine(WaitForGameManagerToLoad());
                }
            }
        }
        public IEnumerator WaitForGameManagerToLoad() {
            yield return new WaitForSeconds(0.5f);
            if (GameManager.instance is null) yield break;
            GameManager.instance.grabBtn.onClick.AddListener(Interact);
            transform.position = GameManager.instance.MainPlayerCamera.transform.position;
            transform.rotation = GameManager.instance.MainPlayerCamera.transform.rotation;
        }
    }
}
