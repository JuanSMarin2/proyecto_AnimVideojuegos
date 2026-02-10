using UnityEngine;

namespace Assets.scripts2
{
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour
    {
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            RegisterComponent();
        }

        // Update is called once per frame
        private void RegisterComponent()
        {
            foreach (ICharacterComponent component in GetComponentsInChildren<ICharacterComponent>())
            {
                component.ParentCharacter = this;
            }
        }
    }
}


