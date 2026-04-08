using UnityEngine.Animations.Rigging;
using Unity.Burst;
using UnityEngine.Animations;
using UnityEngine;
using Unity.Mathematics;
using System.IO;

namespace Assets.Clases.Clase_2.Scripts
{
    [BurstCompile]
    public struct SoftLookAtJob : IWeightedAnimationJob
    {
        public ReadWriteTransformHandle driven;
        public ReadOnlyTransformHandle parent;
        public TransformSceneHandle targetScene;

        public float2 yawLimitsDeg;
        public float2 pitchLimitsDeg;

        public void ProcessAnimation(AnimationStream stream)
        {
        }

        public void ProcessRootMotion(AnimationStream stream)
        {
            float w = jobWeight.Get(stream);

            if (w == 0f) return;

            quaternion parentWorld = parent.IsValid(stream) ? parent.GetRotation(stream) : quaternion.identity;

            float3 drivenWorldPos = driven.GetPosition(stream);
            float3 targetWorldPos = targetScene.GetPosition(stream);

            float3 toTargerWorld = targetWorldPos - drivenWorldPos;

            float len2 = math.lengthsq(toTargerWorld);
            if (len2 < 1e-8f) return;

            float3 dirlocal = math.mul(math.inverse(parentWorld), math.normalize(toTargerWorld));
            float yawDeg = math.degrees(math.atan2(dirlocal.x, dirlocal.z));
            float pitchDeg = math.degrees(math.asin(math.clamp(-dirlocal.y,-1f,1f)));

            yawDeg = math.clamp(yawDeg, yawLimitsDeg.x, yawLimitsDeg.y);
            pitchDeg = math.clamp(pitchDeg, pitchLimitsDeg.x, pitchLimitsDeg.y);

            quaternion clampedLocal = quaternion.EulerXYZ(math.radians(new float3(pitchDeg, yawDeg, 0f)));
            quaternion currentLocal = driven.GetRotation(stream);
            quaternion resultadoTotal = math.slerp(currentLocal, clampedLocal, w);
            driven.SetRotation(stream, resultadoTotal);
        }

        public FloatProperty jobWeight { get; set; }
    }

    [System.Serializable]
    public struct SoftLookAtData : IAnimationJobData
    {
        [SyncSceneToStream] public Transform constrainedObject;
        public Transform target;

        public Vector2 yawLimitsDeg;
        public Vector2 pitchLimitsDeg;

        public bool IsValid() =>
            constrainedObject != null && target != null &&
                                  yawLimitsDeg.x <= yawLimitsDeg.y
                                  && pitchLimitsDeg.x <= pitchLimitsDeg.y;


        public void SetDefaultValues()
        {
            constrainedObject = null;
            target = null;
            yawLimitsDeg = new Vector2(-60f, 60f);
            pitchLimitsDeg = new Vector2(-30f, 30f);
        }
                
    }

    public class SoftLookAtBinder : AnimationJobBinder<SoftLookAtJob, SoftLookAtData>
    {
        public override SoftLookAtJob Create(Animator animator, ref SoftLookAtData data, Component component)
        {
            var job = new SoftLookAtJob
            {
                driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject),
                parent = ReadOnlyTransformHandle.Bind(animator, data.constrainedObject.parent),
                targetScene = animator.BindSceneTransform(data.target),
                yawLimitsDeg = data.yawLimitsDeg,
                pitchLimitsDeg = data.pitchLimitsDeg,
                jobWeight = FloatProperty.Bind(animator, component, "m_Weight")
            };
            return job;
        }
        public override void Destroy(SoftLookAtJob job)
        {
        }
    }
}
