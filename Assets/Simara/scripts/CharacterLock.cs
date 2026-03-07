using Assets.scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Clases.Clase_2.Scripts
{
    public class CharacterLock : MonoBehaviour, ICharacterComponent
    {
        public Character ParentCharacter { get; set; }

        [SerializeField] private Camera camera;
        [SerializeField] private LayerMask detectionMask;
        [SerializeField] private float detectionRadius;
        [SerializeField] private float detectionAngle;
        public void OnLock(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            if (ParentCharacter.LockTarget != null)
            {
                ParentCharacter.LockTarget = null;
                return;
            }

            Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);
            if (detectedObjects.Length == 0) return;

            float nearestAngle = detectionAngle;
            float nearestDistance = detectionRadius;
            int closestObject = 0;

            Vector3 cameraFoward = camera.transform.forward;

            for (int i = 0; i < detectedObjects.Length; i++)
            {
                Collider obj = detectedObjects[i];
                Vector3 objViewDirection = obj.transform.position - camera.transform.position;
                float dot = Vector3.Dot(cameraFoward, objViewDirection.normalized);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if(angle > detectionAngle) continue;

                float distance = Vector3.Distance(obj.transform.position, transform.position);
                if (distance < nearestDistance && angle < nearestDistance)
                    closestObject = 1;

                nearestDistance = Mathf.Min(nearestDistance, distance);
                nearestAngle = Mathf.Min(angle, nearestAngle);
            }
            ParentCharacter.LockTarget = detectedObjects[closestObject].transform;
        }
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
        #endif
    }
}