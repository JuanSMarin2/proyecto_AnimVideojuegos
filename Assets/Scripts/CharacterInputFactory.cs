using UnityEngine;

public static class CharacterInputFactory
{
    

    public static ICharacterInput CreateInput(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Player:
                return new PlayerInput();
            case InputType.Enemy:
                return new EnemyInput();
            default:
                Debug.LogError("Invalid Input Type");
                return null;
        }
    }

    public enum InputType
    {
        Player,
        Enemy
    }

}
