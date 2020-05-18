using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;

namespace Leap.Unity
{
    /// <summary> A skinned and jointed 3D HandModel. </summary>
    public class RiggedHandUtils
    {
        private void DebugJoint(IDictionary<TrackedHandJoint, MixedRealityPose> joints, TrackedHandJoint jointType)
        {
            MixedRealityPose pose;
            if (joints.TryGetValue(jointType, out pose))
            {
                //if (LMHand)
                //{
                //    if (jointType == TrackedHandJoint.Palm)
                //    {
                //        if (pose.Position != LMHand.GetPalmPosition())
                //            Debug.Log("MRTK " + pose.Position + " LM " + LMHand.GetPalmPosition());
                //    }

                //    int fingerIndex = -1;
                //    int boneIndex = -1;
                //    switch (jointType)
                //    {
                //        case TrackedHandJoint.ThumbMetacarpalJoint:
                //            fingerIndex = 0;
                //            boneIndex = 1;
                //            break;
                //        case TrackedHandJoint.ThumbProximalJoint:
                //            fingerIndex = 0;
                //            boneIndex = 2;
                //            break;
                //        case TrackedHandJoint.ThumbDistalJoint:
                //            fingerIndex = 0;
                //            boneIndex = 3;
                //            break;
                //        case TrackedHandJoint.ThumbTip:
                //            fingerIndex = 0;
                //            boneIndex = 4;
                //            break;
                //        case TrackedHandJoint.IndexMetacarpal:
                //            fingerIndex = 1;
                //            boneIndex = 0;
                //            break;
                //        case TrackedHandJoint.IndexKnuckle:
                //            fingerIndex = 1;
                //            boneIndex = 1;
                //            break;
                //        case TrackedHandJoint.IndexMiddleJoint:
                //            fingerIndex = 1;
                //            boneIndex = 2;
                //            break;
                //        case TrackedHandJoint.IndexDistalJoint:
                //            fingerIndex = 1;
                //            boneIndex = 3;
                //            break;
                //        case TrackedHandJoint.IndexTip:
                //            fingerIndex = 1;
                //            boneIndex = 4;
                //            break;
                //        case TrackedHandJoint.MiddleMetacarpal:
                //            fingerIndex = 2;
                //            boneIndex = 0;
                //            break;
                //        case TrackedHandJoint.MiddleKnuckle:
                //            fingerIndex = 2;
                //            boneIndex = 1;
                //            break;
                //        case TrackedHandJoint.MiddleMiddleJoint:
                //            fingerIndex = 2;
                //            boneIndex = 2;
                //            break;
                //        case TrackedHandJoint.MiddleDistalJoint:
                //            fingerIndex = 2;
                //            boneIndex = 3;
                //            break;
                //        case TrackedHandJoint.MiddleTip:
                //            fingerIndex = 2;
                //            boneIndex = 4;
                //            break;
                //        case TrackedHandJoint.RingMetacarpal:
                //            fingerIndex = 3;
                //            boneIndex = 0;
                //            break;
                //        case TrackedHandJoint.RingKnuckle:
                //            fingerIndex = 3;
                //            boneIndex = 1;
                //            break;
                //        case TrackedHandJoint.RingMiddleJoint:
                //            fingerIndex = 3;
                //            boneIndex = 2;
                //            break;
                //        case TrackedHandJoint.RingDistalJoint:
                //            fingerIndex = 3;
                //            boneIndex = 3;
                //            break;
                //        case TrackedHandJoint.RingTip:
                //            fingerIndex = 3;
                //            boneIndex = 4;
                //            break;
                //        case TrackedHandJoint.PinkyMetacarpal:
                //            fingerIndex = 4;
                //            boneIndex = 0;
                //            break;
                //        case TrackedHandJoint.PinkyKnuckle:
                //            fingerIndex = 4;
                //            boneIndex = 1;
                //            break;
                //        case TrackedHandJoint.PinkyMiddleJoint:
                //            fingerIndex = 4;
                //            boneIndex = 2;
                //            break;
                //        case TrackedHandJoint.PinkyDistalJoint:
                //            fingerIndex = 4;
                //            boneIndex = 3;
                //            break;
                //        case TrackedHandJoint.PinkyTip:
                //            fingerIndex = 4;
                //            boneIndex = 4;
                //            break;
                //    }

                //    if (fingerIndex >= 0 && boneIndex >= 0)
                //    {
                //        var LM = LMHand.GetFinger((Finger.FingerType)fingerIndex).bones[boneIndex].position;
                //        var MRTK = fingers[fingerIndex].bones[boneIndex].position;

                //        if (LM.ApproxEquals(MRTK))
                //            Debug.Log("Equal");
                //        else
                //            Debug.Log("MRTK " + MRTK + " LM " + LM);
                //    }
                //}
                //else
                //{
                //    var riggedHands = FindObjectsOfType<RiggedHand>();
                //    foreach (var rh in riggedHands)
                //    {
                //        if (rh.Handedness == Chirality.Left)
                //        {
                //            LMHand = rh;
                //            break;
                //        }
                //    }
                //}
            }
        }
    }
}
