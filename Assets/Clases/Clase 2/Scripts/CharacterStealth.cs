using UnityEngine;
using UnityEngine.InputSystem;

namespace Clases.Clase_2.Scripts
{
    public class CharacterStealth : MonoBehaviour, ICharacterComponent
    {
        public Character ParentCharacter { get; set; }

        [SerializeField] private Animator animator;
        [SerializeField] private GameObject stealthIndicator;

        public bool IsStealth { get; private set; }

        public void OnSneak(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
                SetStealth(true);

            if (ctx.canceled)
                SetStealth(false);
        }

        private void SetStealth(bool value)
        {
            IsStealth = value;

            if (ParentCharacter != null)
                ParentCharacter.IsStealth = value;

            if (animator)
                animator.SetBool("Stealth", value);

            ParentCharacter.IsStealth = !ParentCharacter.IsStealth;

            if (stealthIndicator)
                stealthIndicator.SetActive(value);
        }
    }
}
