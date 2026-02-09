using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.scripts2
{   

    public class CharacterLook: MonoBehaviour, ICharacterComponent
    {
        [Serializable] private Camera camera;
        public void OnLock(InputAction.CallbackContext ctx)
        {
            if (!ctx.started)
            {
                Debug.Log("Lock");
            }
        }
        Character ParentCharacter { get; set; }
    }
}
