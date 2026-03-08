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
        [SerializeField] private float debugDuration;

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
            if (requiereAim && (ParentCharacter == null || !ParentCharacter.IsAiming)) return;
            if (Time.time < _nextShootTime) return;

            _nextShootTime = Time.time + 1f / Mathf.Max(1f, fireRate);

            if (ParentCharacter != null && ParentCharacter.IsStealth)
            {
                Debug.Log("Disparando en sigilo");
            }

            ShootOnce();
        }

        private void ShootOnce()
        {
            if (animator) animator.SetTrigger("Fire");
            if (recoil && (ParentCharacter == null || !ParentCharacter.IsStealth))
            {
                recoil.Kick(camShake, camKick, camRecover);
            }

            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Vector3 from = tracerOrigin ? tracerOrigin.position : ray.origin;

            RaycastHit hit; //  Declaramos aquí

            if (Physics.Raycast(ray, out hit, range, hitMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 to = hit.point;

                Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(ray.origin, to), Color.magenta, debugDuration);
                Debug.DrawLine(from, to, Color.yellow, debugDuration);
                Debug.DrawRay(to, hit.normal, Color.red, debugDuration);

                var info = new IHitttable.HitInfo
                {
                    point = hit.point,
                    normal = hit.normal,
                    damage = 10f
                };

                if (hit.collider.TryGetComponent<IHitttable.IHittable>(out var hittable))
                {
                    hittable.ApplyHit(info);
                }
                else
                {
                    var rb = hit.collider.attachedRigidbody;
                    if (rb && rb.TryGetComponent<IHitttable.IHittable>(out var hittable2))
                    {
                        hittable2.ApplyHit(info);
                    }
                }
            }
            else
            {
                Vector3 to = ray.origin + ray.direction * range;
                Debug.DrawRay(ray.origin, ray.direction * range, Color.gray, debugDuration);
                Debug.DrawLine(from, to, Color.yellow, debugDuration);
            }
        }
    }
}


