using UnityEngine;
using System.Collections;

namespace Clases.Clase_7.Scripts
{
    public class SimpleIK : MonoBehaviour
    {
        [SerializeField] private bool enableIK = true;
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] public Transform lookAtTarget;
        private Animator animator;
        private float weigth;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Pulse(float time = 0.2f)
        {
            StopAllCoroutines();
            StartCoroutine(PulseCR(time));
        }

        private IEnumerator PulseCR(float time)
        {
            float t = 0;
            while (t < time)
            {
                t += Time.deltaTime;
                weigth = Mathf.Clamp01(t / time);
                yield return null;
            }

            t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                weigth = 1f - Mathf.Clamp01(t / time);
                yield return null;
            }

            weigth = 0f;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!enableIK || !animator) return;

            if (lookAtTarget)
            {
                animator.SetLookAtWeight(weigth);
                animator.SetLookAtPosition(lookAtTarget.position);
            }

            if (leftHandTarget)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weigth);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weigth);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            }
        }
    }
}