using System;
using UnityEngine;

namespace Clases.Clase_2.Scripts
{
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour
    {
        private bool isAiming;
        private bool isStealth;
        private Transform lockTarget;
        public bool IsAiming
        {
            get => isAiming;
            set => isAiming = value;
        }        

        public bool IsStealth
        {
            get => isStealth;
            set => isStealth = value;
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