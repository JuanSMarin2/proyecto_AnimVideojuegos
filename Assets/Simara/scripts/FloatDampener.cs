using System;
using UnityEngine;

namespace Assets.scripts
{
    [Serializable]
    public struct FloatDampener
    {
        [SerializeField] private float _smoothTime;
        private float currentVelocity;
        public float TargetValue { get; set; }
        public float CurrentValue { get; private set; }

        public void Update()
        {
            CurrentValue = Mathf.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, _smoothTime);
        }

    }
}
