using UnityEngine;

public class SwapCharacters : MonoBehaviour
{

[SerializeField] private GameObject characterA;
[SerializeField] private GameObject weaponA;
[SerializeField] private GameObject weaponB;
[SerializeField] private GameObject characterB;
[SerializeField] private HealthController healthA;
[SerializeField] private HealthController healthB;

private void Start()
{
    CacheHealthControllers();
    if (characterA.activeSelf)
    {
        weaponA.SetActive(true);
        weaponB.SetActive(false);
        characterB.SetActive(false);
    }
    else
    {
        weaponA.SetActive(false);
        weaponB.SetActive(true);
        characterA.SetActive(false);
    }
}

public void Update()
{
    if (Input.GetKeyDown(KeyCode.Tab))
    {
        Swap();
    }
}

public void Swap()
{
    if (IsActiveCharacterDying())
    {
        return;
    }

    if (characterA.activeSelf)
    {
        weaponA.SetActive(false);
        weaponB.SetActive(true);
        characterA.SetActive(false);
        characterB.SetActive(true);
    }
    else
    {
        weaponA.SetActive(true);
        weaponB.SetActive(false);
        characterA.SetActive(true);
        characterB.SetActive(false);
    }
}

private void CacheHealthControllers()
{
    if (!healthA && characterA)
    {
        healthA = characterA.GetComponentInChildren<HealthController>(true);
    }

    if (!healthB && characterB)
    {
        healthB = characterB.GetComponentInChildren<HealthController>(true);
    }
}

private bool IsActiveCharacterDying()
{
    CacheHealthControllers();

    if (characterA && characterA.activeSelf)
    {
        return healthA && (healthA.IsDying || healthA.IsDead);
    }

    if (characterB && characterB.activeSelf)
    {
        return healthB && (healthB.IsDying || healthB.IsDead);
    }

    return false;
}
}
