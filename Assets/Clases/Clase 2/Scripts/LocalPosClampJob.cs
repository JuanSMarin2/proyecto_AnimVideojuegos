using UnityEngine.Animations.Rigging;
using Unity.Burst;
using UnityEngine.Animations;
using UnityEngine;
using Unity.Mathematics;

namespace Assets.Clases.Clase_2.Scripts
{
    [BurstCompile]
    public struct LocalPosClampJob : IWeightedAnimationJob
    {

        public ReadWriteTransformHandle driven;

        public float3 minLocal;
        public float3 maxLocal;
        public FloatProperty jobWeight { get; set; }

        public void ProcessAnimation(AnimationStream stream)
        {

        }

        public void ProcessRootMotion(AnimationStream stream)
        {
            float w = jobWeight.Get(stream);

            if (w == 0f) return;

            float3 lP = driven.GetLocalPosition(stream);
            float3 clamped = math.clamp(lP, minLocal, maxLocal);
            float3 res= math.lerp(lP, clamped, w);

            driven.SetLocalPosition(stream, res);   
        }
    }

    [System.Serializable]
    public struct LocalPosClamptData : IAnimationJobData
    {
        [SyncSceneToStream] public Transform constrainedObject;

        public Vector3 minLocal;
        public Vector3 maxLocal;

        public bool IsValid() => constrainedObject != null && 
                                 minLocal.x <= maxLocal.x && 
                                 minLocal.y <= maxLocal.y && 
                                 minLocal.z <= maxLocal.z;                                                                     

        public void SetDefaultValues()
        {
            constrainedObject = null;
            minLocal = new Vector3(-0.05f, -0.05f, -0.05f);
            maxLocal = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    public class LocalPosClamptBinder : AnimationJobBinder<LocalPosClampJob, LocalPosClamptData>
    {
        public override LocalPosClampJob Create(Animator animator, ref LocalPosClamptData data, Component component)
        {
            var job = new LocalPosClampJob
            {
                driven = ReadWriteTransformHandle.Bind(animator, data.constrainedObject),
                minLocal = data.minLocal,
                maxLocal = data.maxLocal,
                jobWeight = FloatProperty.Bind(animator, component, name:"m_Weight")
            };
            return job;
        }

        public override void Destroy(LocalPosClampJob job)
        {

        }
    }
}
