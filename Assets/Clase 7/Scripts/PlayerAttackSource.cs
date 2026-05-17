using UnityEngine;

public class PlayerAttackSource : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float heavyMultiplier = 2f;
    [SerializeField] private bool isHeavy;
    [SerializeField] private bool isActive;
    [SerializeField] private AttackController owner;
    [SerializeField] private string playerTag = "Player";

    public bool IsActive => isActive;

    private void OnEnable()
    {
        ResolveOwner();
        if (owner)
        {
            owner.RegisterAttackSource(this);
        }
    }

    private void OnDisable()
    {
        if (owner)
        {
            owner.UnregisterAttackSource(this);
        }
    }

    public float GetDamage()
    {
        float multiplier = isHeavy ? Mathf.Max(1f, heavyMultiplier) : 1f;
        return baseDamage * multiplier;
    }

    public void SetBaseDamage(float value)
    {
        baseDamage = Mathf.Max(0f, value);
    }

    public void SetHeavyMultiplier(float value)
    {
        heavyMultiplier = Mathf.Max(1f, value);
    }

    public void SetHeavy(bool heavy)
    {
        isHeavy = heavy;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    public void SetOwner(AttackController controller)
    {
        owner = controller;
    }

    private void ResolveOwner()
    {
        if (owner)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (!player)
        {
            return;
        }

        owner = player.GetComponent<AttackController>();
    }
}
