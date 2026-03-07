using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.scripts
{
    public class CharacterGun : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Animator animator;
        [SerializeField] private RecoilCameraKick recoil;

        [Header("Shooting")]
        [SerializeField] private bool automatic;
        [SerializeField] private bool requireAim = true;
        [SerializeField] private float fireRate = 10f;
        [SerializeField] private float range = 200f;
        [SerializeField] private LayerMask hitMask;

        [Header("RecoilCamera")]
        [SerializeField] private float camShake = 0.6f;
        [SerializeField] private float camKick = 0.12f;
        [SerializeField] private float camRecover = 0.18f;

        [SerializeField] private Transform traceOrigin;
        
        [SerializeField] private bool isFiring;
        private float _nextShootTime;
        public Character ParentCharacter { get; set; }
        [SerializeField] private float debugDuration;

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.started) isFiring = true;
            if (context.canceled) isFiring = false;
            if (!automatic && context.performed) TryShoot();
        }

        private void Update()
        {
            if (automatic && isFiring) TryShoot();
        }

        private void TryShoot()
        {
            if (requireAim && (ParentCharacter == null || !ParentCharacter.IsAiming)) return;
            if (Time.time < _nextShootTime) return;
            _nextShootTime = Time.time + 1f / Mathf.Max(1f, fireRate);
            //ShootOnce();
        }
        /*
        private void ShootOnce()
        {
            if (animator) animator.SetTrigger("Fire");
            if (recoil) recoil.Kick(camShake, peakDuration: camKick, camRecover);

            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Vector3 from = traceOrigin ? traceOrigin.position : ray.origin;

            if (Physics.Raycast(ray, out var hit, range, hitMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 to = hit.point;

                Debug.DrawRay(start:ray.origin, dir:ray.direction * Vector3.Distance(ray.origin, to), Color.magenta, debugDuration);
                Debug.DrawLine(from, to, Color.yellow, debugDuration);
                Debug.DrawRay(start: to, dir: hit.normal, Color.red, debugDuration);

                var info = new HitInfo
                {
                    point = hit.point,
                    normal = hit.normal,
                    damage = 10f
                };

                if (hit.collider.TryGetComponent<IHittable>(out var h:IHittable))
                {
                    h.ApplyHit(info);
                }
                else
                {
                    var rb:Rigidbody = hit.collider.attachedRigidbody;
                    if (rb && rb.TryGetComponent<IHittable>(out var hRb:IHittable))
                    {
                        hRb.ApplyHit(info);
                    }
                    else
                    {
                        var hParent = hit.collider.GetComponentInParent<IHittable>();
                        if (hParent != null) hParent.ApplyHit(info);
                    }
                }
            }
            else
            {
                Vector3 to = ray.origin + ray.direction * range;
                Debug.DrawRay(start: ray.origin, dir: ray.direction * range, Color.gray, debugDuration);
                Debug.DrawLine(from, to, Color.cyan, debugDuration);
            }
            
        }*/
    }
}
