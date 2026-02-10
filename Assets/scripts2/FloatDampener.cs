using System;
using UnityEngine;

namespace Assets.scripts2
{
    [Serializable]
    public class FloatDampener
    {
        [SerializeField] private float _smoothTime;
        private float _currentvelocity;
        public float targetValue { get; set; }
        public float currentValue { get; private set; }

        public void Update()
        {
            currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref _currentvelocity, _smoothTime);
        }
    }
}
