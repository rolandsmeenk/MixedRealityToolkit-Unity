// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Input
{
    public class RiggedHandVisualizer : MonoBehaviour, IMixedRealityHandVisualizer, IMixedRealitySourceStateHandler, IMixedRealityHandJointHandler
    {
        public virtual Handedness Handedness { get; set; }

        public GameObject GameObjectProxy => gameObject;

        public IMixedRealityController Controller { get; set; }

        //        public Dictionary<TrackedHandJoint, Transform> joints = new Dictionary<TrackedHandJoint, Transform>();

        public Transform Palm;
        public Transform Wrist;
        public Transform ThumbMetacarpalJoint;
        public Transform ThumbProximalJoint;
        public Transform ThumbDistalJoint;
        public Transform ThumbTip;
        public Transform IndexMetacarpal;
        public Transform IndexKnuckle;
        public Transform IndexMiddleJoint;
        public Transform IndexDistalJoint;
        public Transform IndexTip;
        public Transform MiddleMetacarpal;
        public Transform MiddleKnuckle;
        public Transform MiddleMiddleJoint;
        public Transform MiddleDistalJoint;
        public Transform MiddleTip;
        public Transform RingMetacarpal;
        public Transform RingKnuckle;
        public Transform RingMiddleJoint;
        public Transform RingDistalJoint;
        public Transform RingTip;
        public Transform PinkyMetacarpal;
        public Transform PinkyKnuckle;
        public Transform PinkyMiddleJoint;
        public Transform PinkyDistalJoint;
        public Transform PinkyTip;

        private void OnEnable()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
        }

        private void OnDisable()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealitySourceStateHandler>(this);
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityHandJointHandler>(this);
        }

        public bool TryGetJointTransform(TrackedHandJoint joint, out Transform jointTransform)
        {
            jointTransform = null;
            switch (joint)
            {
                case TrackedHandJoint.Palm:
                    jointTransform = Palm;
                    break;
                case TrackedHandJoint.Wrist:
                    jointTransform = Wrist;
                    break;
                case TrackedHandJoint.ThumbMetacarpalJoint:
                    jointTransform = ThumbMetacarpalJoint;
                    break;
                case TrackedHandJoint.ThumbProximalJoint:
                    jointTransform = ThumbProximalJoint;
                    break;
                case TrackedHandJoint.ThumbDistalJoint:
                    jointTransform = ThumbDistalJoint;
                    break;
                case TrackedHandJoint.ThumbTip:
                    jointTransform = ThumbTip;
                    break;
                case TrackedHandJoint.IndexMetacarpal:
                    break;
                case TrackedHandJoint.IndexKnuckle:
                    break;
                case TrackedHandJoint.IndexMiddleJoint:
                    break;
                case TrackedHandJoint.IndexDistalJoint:
                    break;
                case TrackedHandJoint.IndexTip:
                    break;
                case TrackedHandJoint.MiddleMetacarpal:
                    break;
                case TrackedHandJoint.MiddleKnuckle:
                    break;
                case TrackedHandJoint.MiddleMiddleJoint:
                    break;
                case TrackedHandJoint.MiddleDistalJoint:
                    break;
                case TrackedHandJoint.MiddleTip:
                    break;
                case TrackedHandJoint.RingMetacarpal:
                    break;
                case TrackedHandJoint.RingKnuckle:
                    break;
                case TrackedHandJoint.RingMiddleJoint:
                    break;
                case TrackedHandJoint.RingDistalJoint:
                    break;
                case TrackedHandJoint.RingTip:
                    break;
                case TrackedHandJoint.PinkyMetacarpal:
                    break;
                case TrackedHandJoint.PinkyKnuckle:
                    break;
                case TrackedHandJoint.PinkyMiddleJoint:
                    break;
                case TrackedHandJoint.PinkyDistalJoint:
                    break;
                case TrackedHandJoint.PinkyTip:
                    break;
            }

            return (jointTransform != null);
        }

        void IMixedRealitySourceStateHandler.OnSourceDetected(SourceStateEventData eventData) { }

        void IMixedRealitySourceStateHandler.OnSourceLost(SourceStateEventData eventData)
        {
            if (Controller?.InputSource.SourceId == eventData.SourceId)
            {
                Destroy(gameObject);
            }
        }

        void IMixedRealityHandJointHandler.OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
        {
            var inputSystem = CoreServices.InputSystem;

            if (eventData.InputSource.SourceId != Controller.InputSource.SourceId)
            {
                return;
            }
            Debug.Assert(eventData.Handedness == Controller.ControllerHandedness);

            foreach (TrackedHandJoint handJoint in eventData.InputData.Keys)
            {
                Transform jointTransform;
                if (TryGetJointTransform(handJoint, out jointTransform))
                {
                    //if (handJoint == TrackedHandJoint.Palm)
                    {
                        jointTransform.localPosition = eventData.InputData[handJoint].Position;
                    }
                    jointTransform.localRotation = eventData.InputData[handJoint].Rotation;
                }
            }
        }
    }
}