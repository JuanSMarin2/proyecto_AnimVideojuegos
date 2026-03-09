using UnityEngine;
using UnityEngine.InputSystem;

namespace Clases.Clase_2.Scripts
{
    public class CharacterGesture : MonoBehaviour, ICharacterComponent
    {
        public Character ParentCharacter { get; set; }

        [SerializeField] private Animator animator;
        [SerializeField] private string gestureTrigger = "Gesture";

        private bool isPlayingGesture;

        public void OnGesture(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;

            if (!CanPlayGesture()) return;

            animator.SetTrigger(gestureTrigger);
            isPlayingGesture = true;

            Invoke(nameof(ResetGesture), 2f); // duración aproximada de la animación
        }

        private bool CanPlayGesture()
        {
            if (ParentCharacter == null) return false;

            if (ParentCharacter.IsAiming) return false;
            if (ParentCharacter.IsStealth) return false;

            CharacterGun gun = GetComponent<CharacterGun>();
            if (gun != null && gun.IsFiring) return false;

            return true;
        }

        private void ResetGesture()
        {
            isPlayingGesture = false;
        }
    }
}
