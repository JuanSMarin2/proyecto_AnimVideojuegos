using Assets.Clases.Clase_2.Scripts;
using System;
using System.Xml.Serialization;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace Clases.Clase_2.Scripts
{
    public class CharacterGun : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Animator animator;
        [SerializeField] private RecoilCameraKick recoil;

        [Header("Shooting")]
        [SerializeField] private bool automatic;
        [SerializeField] private bool requiereAim = true;
        [SerializeField] private float fireRate = 10f;
        [SerializeField] private float range = 200f;
        [SerializeField] private LayerMask hitMask;

        [Header("RecoilCamera")]
        private float camShake = 0.6f;
        [SerializeField] private float camKick = 0.12f;
        [SerializeField] private float camRecover = 0.18f;

        [SerializeField] private Transform tracerOrigin;



        [SerializeField] private bool isFiring;

        public Character ParentCharacter { get; set; }
        
        private float _nextShootTime;
        public void OnFire(InputAction.CallbackContext context)
        {
            if(context.started) isFiring = true;
            if (context.canceled) isFiring = false;
            if(!automatic && context.performed) Tryshoot();

        }

        private void Update()
        {
            if (automatic && isFiring){
                Tryshoot();
            }
        }

        private void Tryshoot()
        {
            if(requiereAim && (ParentCharacter == null || !ParentCharacter.IsAiming)) return;
            if (Time.time < _nextShootTime) return;
            _nextShootTime = Time.time + 1f / Mathf.Max(1f, fireRate);
            ShootOnce();
        }

        private void ShootOnce()
        {
            if(animator) animator.SetTrigger(name:"Fire");
            
        }
    }
}


