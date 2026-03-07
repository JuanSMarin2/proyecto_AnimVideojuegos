using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    public static class CharacterInputFactori
    {
        public static ICgharacterInput CreateInput(InputType type)
        {
            switch (type)
            {
                case InputType.Player:
                    return new PlayerInput();
                case InputType.Enemy:
                    return new EnemyInput();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public enum InputType
        {
            Player,
            Enemy
        }
    }
}
