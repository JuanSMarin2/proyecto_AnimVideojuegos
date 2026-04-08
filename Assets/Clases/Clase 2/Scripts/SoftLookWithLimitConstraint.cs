using UnityEngine.Animations.Rigging;
using Unity.Burst;
using UnityEngine.Animations;
using UnityEngine;
using Unity.Mathematics;

namespace Assets.Clases.Clase_2.Scripts
{
    [AddComponentMenu("Animation Rigging/Soft Look With Limit Constraint")]
    public class SoftLookWithLimitConstraint: RigConstraint<SoftLookAtJob, SoftLookAtData, SoftLookAtBinder>
    {
    }
}
