/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2020.                                   *
 * Ultraleap proprietary and confidential.                                    *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leap;
using Leap.Unity.Attributes;
using UnityEngine.Serialization;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

namespace Leap.Unity
{
    /// <summary> A skinned and jointed 3D HandModel. </summary>
    public class RiggedHandMRTK : MonoBehaviour, IMixedRealityControllerVisualizer, IMixedRealitySourceStateHandler, IMixedRealityHandJointHandler
    {
        /** The number of fingers on a hand.*/
        public const int NUM_FINGERS = 5;

        /** The model width of the hand in meters. This value is used with the measured value
        * of the user's hand to scale the model proportionally.
        */
        // public float handModelPalmWidth = 0.085f;
        /** The array of finger objects for this hand. The array is ordered from thumb (element 0) to pinky (element 4).*/
        public RiggedFingerMRTK[] fingers = new RiggedFingerMRTK[NUM_FINGERS];

        // Unity references
        /** Transform object for the palm object of this hand. */
        public Transform palm;
        /** Transform object for the wrist joint of this hand. */
        public Transform wristJoint;

        [SerializeField]
        [OnEditorChange("deformPositionsInFingers")]
        private bool _deformPositionsInFingers = true;
        public bool deformPositionsInFingers
        {
            get { return _deformPositionsInFingers; }
            set
            {
                _deformPositionsInFingers = value;
            }
        }

        [Tooltip("Because bones only exist at their roots in model rigs, the length " +
          "of the last fingertip bone is lost when placing bones at positions in the " +
          "tracked hand. " +
          "This option scales the last bone along its X axis (length axis) to match " +
          "its bone length to the tracked bone length. This option only has an " +
          "effect if Deform Positions In Fingers is enabled.")]
        [DisableIf("_deformPositionsInFingers", isEqualTo: false)]
        [SerializeField]
        [OnEditorChange("scaleLastFingerBones")]
        private bool _scaleLastFingerBones = true;
        public bool scaleLastFingerBones
        {
            get { return _scaleLastFingerBones; }
            set
            {
                _scaleLastFingerBones = value;
            }
        }

        [Tooltip("Hands are typically rigged in 3D packages with the palm transform near the wrist. Uncheck this if your model's palm transform is at the center of the palm similar to Leap API hands.")]
        [FormerlySerializedAs("ModelPalmAtLeapWrist")]
        public bool modelPalmAtLeapWrist = true;

        [Tooltip("Set to True if each finger has an extra trasform between palm and base of the finger.")]
        [FormerlySerializedAs("UseMetaCarpals")]
        public bool useMetaCarpals;

        [Tooltip("If non-zero, this vector and the modelPalmFacing vector " +
          "will be used to re-orient the Transform bones in the hand rig, to " +
          "compensate for bone axis discrepancies between Leap Bones and model " +
          "bones.")]
        public Vector3 modelFingerPointing = new Vector3(0, 0, 0);
        [Tooltip("If non-zero, this vector and the modelFingerPointing vector " +
          "will be used to re-orient the Transform bones in the hand rig, to " +
          "compensate for bone axis discrepancies between Leap Bones and model " +
          "bones.")]
        public Vector3 modelPalmFacing = new Vector3(0, 0, 0);

        /// <summary> Rotation derived from the `modelFingerPointing` and
        /// `modelPalmFacing` vectors in the RiggedHand inspector. </summary>
        public Quaternion userBoneRotation
        {
            get
            {
                if (modelFingerPointing == Vector3.zero || modelPalmFacing == Vector3.zero)
                {
                    return Quaternion.identity;
                }
                return Quaternion.Inverse(Quaternion.LookRotation(modelFingerPointing, -modelPalmFacing));
            }
        }

        private void UpdateFingerSettings()
        {
            var riggedFingers = GetComponentsInChildren<RiggedFingerMRTK>();
            foreach (var finger in riggedFingers)
            {
                finger.deformPosition = deformPositionsInFingers;
                finger.scaleLastFingerBone = scaleLastFingerBones;
                finger.SetupRiggedFinger(useMetaCarpals);
            }
        }

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

        // IMixedRealityControllerVisualizer
        public GameObject GameObjectProxy => gameObject;

        public IMixedRealityController Controller { get; set; }

        // IMixedRealitySourceStateHandler
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            UpdateFingerSettings();
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            if (Controller?.InputSource.SourceId == eventData.SourceId)
            {
                Destroy(gameObject);
            }
        }

        public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
        {
            var inputSystem = CoreServices.InputSystem;

            if (eventData.InputSource.SourceId != Controller.InputSource.SourceId)
            {
                return;
            }

            Debug.Assert(eventData.Handedness == Controller.ControllerHandedness);

            if (palm != null)
            {
                MixedRealityPose pose;
                if (modelPalmAtLeapWrist)
                {
                    if (eventData.InputData.TryGetValue(TrackedHandJoint.Wrist, out pose))
                    {
                        palm.position = pose.Position;
                    }

                    if (eventData.InputData.TryGetValue(TrackedHandJoint.Palm, out pose))
                    {
                        palm.rotation = pose.Rotation * userBoneRotation;
                    }
                }
                else
                {
                    if (eventData.InputData.TryGetValue(TrackedHandJoint.Palm, out pose))
                    {
                        palm.position = pose.Position;
                        palm.rotation = pose.Rotation * userBoneRotation;
                    }

                    if (wristJoint)
                    {
                        if (eventData.InputData.TryGetValue(TrackedHandJoint.Wrist, out pose))
                        {
                            wristJoint.position = pose.Position; 
                        }
                    }                    
                }
            }

            for (int i = 0; i < fingers.Length; ++i)
            {
                if (fingers[i] != null)
                {
                    fingers[i].HandJointsUpdated(eventData, transform.lossyScale.x);
                }
            }
        }
    }
}
