using UnityEngine;

namespace Assets.scripts
{
    internal class PlayerInput : ICgharacterInput
    {
        public float GetSpeedInput()
        {
            return new Vector2(
                UnityEngine.Input.GetAxis("Horizontal"),
                UnityEngine.Input.GetAxis("Vertical")
            ).magnitude;
        }
    }
}
