using UnityEngine;

public class PlayerInput : ICharacterInput
{


  public  float GetSpeedInput()
    {
        return new Vector2 (
           UnityEngine.Input.GetAxis("Horizontal"),
              UnityEngine.Input.GetAxis("Vertical")
              ).magnitude;


    }
}
