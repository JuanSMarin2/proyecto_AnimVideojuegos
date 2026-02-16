using UnityEngine;
using UnityEngine.InputSystem;

namespace Clases.Clase_2.Scripts
{
    public class CharacterLock: MonoBehaviour, ICharacterComponent
    {
        public Character ParentCharacter { get; set; }

        public void OnLock(InputAction.CallbackContext ctx)
        {
            
        }
    }
}