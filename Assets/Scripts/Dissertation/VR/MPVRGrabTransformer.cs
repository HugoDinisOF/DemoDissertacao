using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_ANDROID
    using UnityEngine.XR.Interaction.Toolkit;
    using UnityEngine.XR.Interaction.Toolkit.Transformers;
#endif
using Dissertation.Core;

#if !UNITY_ANDROID
    public class MPVRGrabTransformer : XRGeneralGrabTransformer
    {
        public override void OnGrab(XRGrabInteractable grabInteractable)
        {
            var virtualObject = grabInteractable.GetComponent<LeanDragTranslateExt>();
            if (virtualObject.ChangeOwnership()) 
            { 
                base.OnGrab(grabInteractable);
            }
        }

        public void OnLeaveGrab(SelectExitEventArgs selectExitEventArgs) {
            var virtualObject = selectExitEventArgs.interactable.GetComponent<LeanDragTranslateExt>();
            virtualObject.RemoveOwnership();
        }
    }
#else
    public class MPVRGrabTransformer : MonoBehaviour { }
#endif
