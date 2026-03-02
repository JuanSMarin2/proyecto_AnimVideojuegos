using Assets.Clases.Clase_2.Scripts;
using System;
using System.Xml.Serialization;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using static Assets.Clases.Clase_2.Scripts.IHitttable;

namespace Assets.Clases.Clase_2.Scripts
{
    public class HitReceiver : MonoBehaviour, IHitttable.IHittable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string hitTrigerName = "Hit";
        public void ApplyHit(HitInfo info)
        {
            if (_animator)
            {
                _animator.SetTrigger(hitTrigerName);
            }
        }

        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
