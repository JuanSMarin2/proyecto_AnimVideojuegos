using UnityEngine;
using System.Collections;

public abstract class PowerUpDecorator : MonoBehaviour
{
    [SerializeField] protected float duration = 8f;

    public abstract void Apply();

    public abstract void Remove();

    public IEnumerator PowerUpRoutine()
    {
        Apply();

        yield return new WaitForSeconds(duration);

        Remove();

        Destroy(this);
    }
}