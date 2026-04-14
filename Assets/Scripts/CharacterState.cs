using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [SerializeField] private float _startStamina = 1000f;
    [SerializeField] private float _staminaRegen = 0f;
    [SerializeField] private float _currentStamina = 100f;
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private float _currentHealth = 100f;

    public float CurrentStamina => _currentStamina;

    private void Start()
    {
        _currentStamina = _startStamina;
        _currentHealth = _startHealth;

    }

    private void Update()
    {
        RegenerateStamina(_staminaRegen * Time.deltaTime);
    }

    private void RegenerateStamina(float staminaRegen)
    {
        _currentStamina = Mathf.Clamp(CurrentStamina + staminaRegen, 0, _startStamina);
    }


    private float GetStaminaDepletion() => 10f;
   


    public void DepleteStamina(float staminaDepletion)
    {
        _currentStamina = CurrentStamina - GetStaminaDepletion() * staminaDepletion;
    }


    public void DepleteHealth(float healthDepletion, out bool zeroHealth)
    {
        _currentHealth -= healthDepletion;
        zeroHealth = false;
        if (_currentHealth <= 0)
        {
            zeroHealth = true;
            _currentHealth = 0;
        }
    }
}