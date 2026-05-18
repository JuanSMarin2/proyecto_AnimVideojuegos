using UnityEngine;

public class SwapCharacters : MonoBehaviour
{

[SerializeField] private GameObject characterA;
[SerializeField] private GameObject weaponA;
[SerializeField] private GameObject weaponB;
[SerializeField] private GameObject characterB;

private void Start()
{
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
}
