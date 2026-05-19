using UnityEngine;
using System.Collections;

public abstract class PowerUpDecorator : MonoBehaviour
{
    [SerializeField] protected float duration = 8f;

    private float endTime;
    private bool hasApplied;
    private bool hasRemoved;

    public float Duration => duration;
    public float RemainingTime => Mathf.Max(0f, endTime - Time.time);
    public bool IsActive => hasApplied && !hasRemoved && RemainingTime > 0f;

    public abstract void Apply();

    public abstract void Remove();

    public IEnumerator PowerUpRoutine()
    {
        endTime = Time.time + duration;

        Apply();
        hasApplied = true;

        while (RemainingTime > 0f)
        {
            yield return null;
        }

        ForceRemove();
    }

    public void Refresh()
    {
        if (hasRemoved)
        {
            return;
        }

        endTime = Time.time + duration;
    }

    private void OnDisable()
    {
        ForceRemove();
    }

    private void OnDestroy()
    {
        ForceRemove();
    }

    private void ForceRemove()
    {
        if (!hasApplied || hasRemoved)
        {
            return;
        }

        hasRemoved = true;
        Remove();
        Destroy(this);
    }
}