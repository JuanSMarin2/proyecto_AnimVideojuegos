using System;
using UnityEngine;

namespace Clases.Clase_2.Scripts
{
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour
    {
        private bool isAiming;

        public bool IsAiming
        {
            get => isAiming;
            set => isAiming = value;
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