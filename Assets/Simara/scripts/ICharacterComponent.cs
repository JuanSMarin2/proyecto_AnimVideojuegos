using UnityEngine;

namespace Assets.scripts
{
    public interface ICharacterComponent
    {
        Character ParentCharacter { get; set; }
    }
}