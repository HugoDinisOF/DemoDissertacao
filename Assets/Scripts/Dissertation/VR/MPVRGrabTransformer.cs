using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using Dissertation.Core;

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
