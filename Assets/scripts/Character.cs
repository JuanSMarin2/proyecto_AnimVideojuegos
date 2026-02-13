using System;
using UnityEngine;

namespace Assets.scripts
{
    [DefaultExecutionOrder(-1)]

    public class Character : MonoBehaviour
{
        private void Awake()
        {
            RegisterComponents();
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
