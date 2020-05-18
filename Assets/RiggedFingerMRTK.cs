using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    /// <summary>
    /// Manages the position and orientation of the bones in a model rigged for skeletal animation.
    ///  
    /// The class expects that the graphics model's bones that correspond to the bones in
    /// the Leap Motion hand model are in the same order in the bones array.
    /// </summary>
    public class RiggedFingerMRTK : MonoBehaviour
    {
        public Finger.FingerType fingerType = Finger.FingerType.TYPE_INDEX;

        /** The number of bones in a finger. */
        public const int NUM_BONES = 4;

        /** The number of joints in a finger. */
        public const int NUM_JOINTS = 3;

        /** Bones positioned and rotated by FingerModel. */
        public Transform[] bones = new Transform[NUM_BONES];

        List<Tuple<Finger.FingerType, int, TrackedHandJoint>> jointTypes = new List<Tuple<Finger.FingerType, int, TrackedHandJoint>>()
        {
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_THUMB, 1, TrackedHandJoint.ThumbMetacarpalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_THUMB, 2, TrackedHandJoint.ThumbProximalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_THUMB, 3, TrackedHandJoint.ThumbDistalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_THUMB, 4, TrackedHandJoint.ThumbTip),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_INDEX, 0, TrackedHandJoint.IndexMetacarpal),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_INDEX, 1, TrackedHandJoint.IndexKnuckle),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_INDEX, 2, TrackedHandJoint.IndexMiddleJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_INDEX, 3, TrackedHandJoint.IndexDistalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_INDEX, 4, TrackedHandJoint.IndexTip),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_MIDDLE, 0, TrackedHandJoint.MiddleMetacarpal),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_MIDDLE, 1, TrackedHandJoint.MiddleKnuckle),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_MIDDLE, 2, TrackedHandJoint.MiddleMiddleJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_MIDDLE, 3, TrackedHandJoint.MiddleDistalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_MIDDLE, 4, TrackedHandJoint.MiddleTip),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_RING, 0, TrackedHandJoint.RingMetacarpal),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_RING, 1, TrackedHandJoint.RingKnuckle),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_RING, 2, TrackedHandJoint.RingMiddleJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_RING, 3, TrackedHandJoint.RingDistalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_RING, 4, TrackedHandJoint.RingTip),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_PINKY, 0, TrackedHandJoint.PinkyMetacarpal),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_PINKY, 1, TrackedHandJoint.PinkyKnuckle),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_PINKY, 2, TrackedHandJoint.PinkyMiddleJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_PINKY, 3, TrackedHandJoint.PinkyDistalJoint),
            new Tuple<Finger.FingerType, int, TrackedHandJoint>(Finger.FingerType.TYPE_PINKY, 4, TrackedHandJoint.PinkyTip),
        };


        /// <summary>
        /// Fingertip lengths for the standard edit-time hand.
        /// </summary>
        private static float[] s_standardFingertipLengths = null;
        static RiggedFingerMRTK()
        {
            // Calculate standard fingertip lengths.
            s_standardFingertipLengths = new float[5];
            var testHand = TestHandFactory.MakeTestHand(isLeft: true, unitType: TestHandFactory.UnitType.UnityUnits);
            for (int i = 0; i < 5; i++)
            {
                var fingertipBone = testHand.Fingers[i].bones[3];
                s_standardFingertipLengths[i] = fingertipBone.Length;
            }
        }

        /// <summary>
        /// Allows the mesh to be stretched to align with finger joint positions.
        /// Only set to true when mesh is not visible.
        /// </summary>
        [HideInInspector]
        public bool deformPosition = false;

        [HideInInspector]
        public bool scaleLastFingerBone = false;

        public Vector3 modelFingerPointing = Vector3.forward;
        public Vector3 modelPalmFacing = -Vector3.up;

        public Quaternion Reorientation()
        {
            return Quaternion.Inverse(Quaternion.LookRotation(modelFingerPointing, -modelPalmFacing));
        }

        /** Returns the location of the given joint on the finger */
        public MixedRealityPose GetJointPose(int fingerBoneIndex, IDictionary<TrackedHandJoint, MixedRealityPose> joints)
        {
            MixedRealityPose pose;
            TrackedHandJoint jointType = jointTypes.Find(a => a.Item1 == this.fingerType && a.Item2 == fingerBoneIndex).Item3;
            joints.TryGetValue(jointType, out pose);
            return pose;
        }

        public void HandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData, float parentRiggedHandScale = 1f)
        {
            for (int i = 0; i < bones.Length; ++i)
            {
                if (bones[i] != null)
                {
                    bones[i].rotation = GetJointPose(i, eventData.InputData).Rotation * Reorientation();
                    if (deformPosition)
                    {
                        var boneRootPos = GetJointPose(i, eventData.InputData).Position;
                        bones[i].position = boneRootPos;

                        if (i == 3 && scaleLastFingerBone)
                        {
                            // Set fingertip base bone scale to match the bone length to the fingertip.
                            // This will only scale correctly if the model was constructed to match
                            // the standard "test" edit-time hand model from the TestHandFactory.
                            var boneTipPos = GetJointPose(i + 1, eventData.InputData).Position;
                            var boneVec = boneTipPos - boneRootPos;

                            if (parentRiggedHandScale != 0f && parentRiggedHandScale != 1f)
                            {
                                boneVec /= parentRiggedHandScale;
                            }
                            var boneLen = boneVec.magnitude;
                            var standardLen = s_standardFingertipLengths[(int)this.fingerType];
                            var newScale = bones[i].transform.localScale;
                            var lengthComponentIdx = getLargestComponentIndex(modelFingerPointing);
                            newScale[lengthComponentIdx] = boneLen / standardLen;
                            bones[i].transform.localScale = newScale;
                        }
                    }
                }
            }
        }

        private int getLargestComponentIndex(Vector3 pointingVector)
        {
            var largestValue = 0f;
            var largestIdx = 0;
            for (int i = 0; i < 3; i++)
            {
                var testValue = pointingVector[i];
                if (Mathf.Abs(testValue) > largestValue)
                {
                    largestIdx = i;
                    largestValue = Mathf.Abs(testValue);
                }
            }
            return largestIdx;
        }

        public void SetupRiggedFinger(bool useMetaCarpals)
        {
            findBoneTransforms(useMetaCarpals);
            modelFingerPointing = calulateModelFingerPointing();
        }

        private void findBoneTransforms(bool useMetaCarpals)
        {
            if (!useMetaCarpals || fingerType == Finger.FingerType.TYPE_THUMB)
            {
                bones[1] = transform;
                bones[2] = transform.GetChild(0).transform;
                bones[3] = transform.GetChild(0).transform.GetChild(0).transform;
            }
            else
            {
                bones[0] = transform;
                bones[1] = transform.GetChild(0).transform;
                bones[2] = transform.GetChild(0).transform.GetChild(0).transform;
                bones[3] = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform;
            }
        }

        private Vector3 calulateModelFingerPointing()
        {
            Vector3 distance = transform.InverseTransformPoint(transform.position) - transform.InverseTransformPoint(transform.GetChild(0).transform.position);
            Vector3 zeroed = RiggedHand.CalculateZeroedVector(distance);
            return zeroed;
        }
    }
}
