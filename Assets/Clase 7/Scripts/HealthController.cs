using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private int animatorLayerIndex = 0;
    [SerializeField] private string damageStateTag = "Damage";
    [SerializeField] private string[] damageStateNames;

    [Header("Death")]
    [SerializeField] private float deathReloadDelay = 3f;
    [SerializeField] private MonoBehaviour[] disableOnDeath;
    [SerializeField] private Rigidbody rootRigidbody;
    [SerializeField] private bool setKinematicOnDeath = true;

    private bool isDead;
    private bool deathRoutineRunning;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        if (!rootRigidbody)
        {
            rootRigidbody = GetComponent<Rigidbody>();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void ApplyDamage(DamageInfo info)
    {
        if (isDead)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - info.Amount, 0f, maxHealth);

        if (currentHealth <= 0f && !deathRoutineRunning)
        {
            StartCoroutine(DeathSequence(info.Direction));
        }
    }

    private IEnumerator DeathSequence(HitDirection lastHitDirection)
    {
        deathRoutineRunning = true;
        isDead = true;

        if (animator && IsInDamageState())
        {
            while (IsInDamageState())
            {
                yield return null;
            }
        }

        DisableMovement();
        TriggerDeathAnimation(lastHitDirection);

        yield return new WaitForSeconds(deathReloadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool IsInDamageState()
    {
        if (!animator)
        {
            return false;
        }

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(animatorLayerIndex);

        if (!string.IsNullOrWhiteSpace(damageStateTag) && state.IsTag(damageStateTag))
        {
            return true;
        }

        if (damageStateNames != null)
        {
            for (int i = 0; i < damageStateNames.Length; i++)
            {
                string stateName = damageStateNames[i];
                if (!string.IsNullOrWhiteSpace(stateName) && state.IsName(stateName))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void TriggerDeathAnimation(HitDirection lastHitDirection)
    {
        if (!animator)
        {
            return;
        }

        if (lastHitDirection == HitDirection.Back)
        {
            animator.SetTrigger("DieBack");
        }
        else
        {
            animator.SetTrigger("Die");
        }
    }

    private void DisableMovement()
    {
        if (disableOnDeath != null)
        {
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                MonoBehaviour behaviour = disableOnDeath[i];
                if (behaviour)
                {
                    behaviour.enabled = false;
                }
            }
        }

        if (rootRigidbody)
        {
            rootRigidbody.linearVelocity = Vector3.zero;
            rootRigidbody.angularVelocity = Vector3.zero;

            if (setKinematicOnDeath)
            {
                rootRigidbody.isKinematic = true;
            }
        }
    }
}
