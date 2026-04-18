using System;
using System.Linq;
using UnityEngine;

namespace Clases.Clase_7.Scripts
{
    public class RagDollController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody rootRB;
        [SerializeField] private Collider rootCol;
        [SerializeField] private Transform hips;
        [SerializeField] private GameObject damageHitboxes;

        private Rigidbody[] bonesRBs;
        private Collider[] boneCols;
        private bool ragdollOn;

        private void Awake()
        {
            if (!_animator) _animator = GetComponent<Animator>();
            if (!rootRB) rootRB = GetComponentInParent<Rigidbody>();
            if (!rootCol) rootCol = GetComponent<Collider>();

            var allRBsUnderAnimator = GetComponentsInChildren<Rigidbody>(includeInactive: true);
            bonesRBs = allRBsUnderAnimator.Where(r => r != rootRB).ToArray();
            boneCols = bonesRBs.Select(r => r.GetComponent<Collider>()).Where(c => c != null).ToArray();

            SetAnimatedState();
        }

        private void SetAnimatedState()
        {
            ragdollOn = false;

            // Activamos las hitboxes de daño cuando el personaje está animado
            if (damageHitboxes) damageHitboxes.SetActive(true);

            foreach (var rb in bonesRBs)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            foreach (var c in boneCols)
            {
                c.enabled = false;
            }

            if (rootCol) rootCol.enabled = true;
            if (rootRB) rootRB.isKinematic = false;
            if (_animator) _animator.enabled = true;
        }

        public void EnableRagDoll()
        {
            if (ragdollOn) return;
            ragdollOn = true;

            // Desactivamos las hitboxes de daño para que no pueda atacar mientras es un ragdoll
            if (damageHitboxes) damageHitboxes.SetActive(false);

            if (_animator) _animator.enabled = false;

            if (rootCol) rootCol.enabled = false;
            if (rootRB)
            {
                rootRB.isKinematic = true;
                rootRB.linearVelocity = Vector3.zero;
                rootRB.angularVelocity = Vector3.zero;
            }

            foreach (var rb in bonesRBs)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }

            foreach (var c in boneCols)
            {
                c.enabled = true;
            }

            Physics.SyncTransforms();
        }

        public void DisableRagDollAndRevive()
        {
            SetAnimatedState();
            if (hips) transform.position = hips.position;
        }
    }
}