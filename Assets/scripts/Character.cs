using System;
using UnityEngine;

namespace Assets.scripts
{
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour
    {
        private bool isAiming;
        private Transform lockTarget;
        public bool IsAiming
        {
            get => isAiming;
            set => isAiming = value;
        }

        public Transform LockTarget
        {
            get => lockTarget;
            set => lockTarget = value;
        }

        private void Awake()
        {
            RegisterComponents();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void RegisterComponents()
        {
            foreach (ICharacterComponent component in GetComponentsInChildren<ICharacterComponent>())
            {
                component.ParentCharacter = this;
            }
        }
    }

}
