using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.WindowsMixedReality;
using Microsoft.Windows.Graphics.Holographic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetFocusPoint : MonoBehaviour
{
    public GameObject focusedObject;


    void Update()
    {
        // Normally the normal is best set to be the opposite of the main camera's 
        // forward vector.
        // If the content is actually all on a plane (like text), set the normal to 
        // the normal of the plane and ensure the user does not pass through the 
        // plane.
        var normal = -Camera.main.transform.forward;
        var position = focusedObject.transform.position;
        UnityEngine.XR.WSA.HolographicSettings.SetFocusPointForFrame(position, normal);
    }

    bool renderFromPV = false;
    public void TogglePhotoVideoCamera()
    {
        renderFromPV = !renderFromPV;

#if WINDOWS_UWP
        // If the default display has configuration for a PhotoVideoCamera, we want to enable it
        HolographicViewConfiguration viewConfiguration = HolographicDisplay.GetDefault()?.TryGetViewConfiguration(HolographicViewConfigurationKind.PhotoVideoCamera);
        if (viewConfiguration != null)
        {
            viewConfiguration.IsEnabled = renderFromPV;
        }
#endif // WINDOWS_UWP
    }
}
