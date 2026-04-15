using UnityEngine;

public class AttackHitBoxController : MonoBehaviour
{
    [SerializeField] private GameObject[] hitBoxes;

    public void ToggleHitboxes(int attackId)
    {

        for(int hitBoxIndex = 0; hitBoxIndex < hitBoxes.Length; hitBoxIndex++)
        {
            GameObject hitBox = this.hitBoxes[hitBoxIndex];
            hitBox.SetActive(!hitBox.activeSelf);
        }

    }

    public void CleanupHitBoxes()
    {
        foreach (GameObject colliders in hitBoxes)
        {
            colliders.SetActive(false);
        }
    }
}
