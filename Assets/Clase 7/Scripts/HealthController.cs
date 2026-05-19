using System.Collections;
using UnityEngine;
using Clases.Clase_8.Scripts;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private bool isPlayer = false;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private int animatorLayerIndex = 0;
    [SerializeField] private string damageStateTag = "Damage";
    [SerializeField] private string[] damageStateNames;
    [SerializeField] private string movementXParam = "SpeedX";
    [SerializeField] private string movementYParam = "SpeedY";
    [SerializeField] private bool forceCrossfadeOnDeath = false;
    [SerializeField] private float deathCrossfadeDuration = 0.05f;

    [Header("Death")]
    [SerializeField] private float deathReloadDelay = 3f;
    [SerializeField] private bool reloadSceneOnDeath = true;
    [SerializeField] private bool notifyOnDeath = false;
    [SerializeField] private bool disableCombatOnDeath = true;
    [SerializeField] private bool resetMovementParamsOnDeath = true;
    [SerializeField] private MonoBehaviour[] disableOnDeath;
    [SerializeField] private Rigidbody rootRigidbody;
    [SerializeField] private bool setKinematicOnDeath = true;

    private bool isDead;
    private bool deathRoutineRunning;
    private bool hasMovementXParam;
    private bool hasMovementYParam;
    private static float sharedPlayerHealth;
    private static bool sharedPlayerHealthSet;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;
    public bool IsDying => deathRoutineRunning;

    public void AddMaxHealth(float amount, bool healToMax = true)
    {
        if (Mathf.Approximately(amount, 0f))
        {
            return;
        }

        maxHealth = Mathf.Max(1f, maxHealth + amount);

        if (healToMax)
        {
            SetCurrentHealth(maxHealth);
        }
        else
        {
            SetCurrentHealth(currentHealth + amount);
        }
    }

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>(true);
        }

        if (!rootRigidbody)
        {
            rootRigidbody = GetComponent<Rigidbody>();
        }

        CacheAnimatorParams();

        SetCurrentHealth(currentHealth);
    }

    private void OnEnable()
    {
        if (isPlayer && !sharedPlayerHealthSet)
        {
            sharedPlayerHealth = currentHealth;
            sharedPlayerHealthSet = true;
        }
    }

    private void LateUpdate()
    {
        if (!isPlayer)
        {
            return;
        }

        // Keep all player health values in sync each frame.
        if (!sharedPlayerHealthSet)
        {
            sharedPlayerHealth = currentHealth;
            sharedPlayerHealthSet = true;
        }
        else if (!Mathf.Approximately(currentHealth, sharedPlayerHealth))
        {
            currentHealth = sharedPlayerHealth;
        }
    }

    public void ApplyDamage(DamageInfo info)
    {
        if (isDead)
        {
            return;
        }

        SetCurrentHealth(currentHealth - info.Amount);

        if (currentHealth <= 0f && !deathRoutineRunning)
        {
            StartCoroutine(DeathSequence(info.Direction));
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
        {
            return;
        }

        SetCurrentHealth(currentHealth + Mathf.Max(0f, amount));
    }

    public void HealPercent(float percent)
    {
        if (isDead)
        {
            return;
        }

        float clampedPercent = Mathf.Clamp01(percent);
        Heal(maxHealth * clampedPercent);
    }

    private void SetCurrentHealth(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, maxHealth);
        currentHealth = clamped;

        if (isPlayer)
        {
            sharedPlayerHealth = clamped;
            sharedPlayerHealthSet = true;
        }
    }

    private IEnumerator DeathSequence(HitDirection lastHitDirection)
    {
        deathRoutineRunning = true;
        isDead = true;

        DisableMovement();
        if (disableCombatOnDeath)
        {
            DisableCombat();
        }
        if (forceCrossfadeOnDeath)
        {
            TriggerDeathAnimation(lastHitDirection, true);
        }
        else
        {
            yield return WaitForAnimatorReady();
            TriggerDeathAnimation(lastHitDirection, false);
        }
        if (notifyOnDeath)
        {
            SendMessage("EnemyDefeated", SendMessageOptions.DontRequireReceiver);
        }
        if (reloadSceneOnDeath)
        {
            yield return new WaitForSeconds(deathReloadDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator WaitForAnimatorReady()
    {
        if (!animator)
        {
            yield break;
        }

        while (animator.IsInTransition(animatorLayerIndex) || IsInDamageState())
        {
            yield return null;
        }
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

    private void TriggerDeathAnimation(HitDirection lastHitDirection, bool forceCrossfade)
    {
        if (!animator)
        {
            return;
        }

        if (lastHitDirection == HitDirection.Back)
        {
            animator.SetBool("DieBack", true);
            animator.SetBool("Die", false);
            if (forceCrossfade)
            {
                animator.CrossFade("DieBack", Mathf.Max(0f, deathCrossfadeDuration), animatorLayerIndex);
            }
        }
        else
        {
            animator.SetBool("Die", true);
            animator.SetBool("DieBack", false);
            if (forceCrossfade)
            {
                animator.CrossFade("Die", Mathf.Max(0f, deathCrossfadeDuration), animatorLayerIndex);
            }
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
            if (!rootRigidbody.isKinematic)
            {
                rootRigidbody.linearVelocity = Vector3.zero;
                rootRigidbody.angularVelocity = Vector3.zero;
            }

            if (setKinematicOnDeath)
            {
                rootRigidbody.isKinematic = true;
            }
        }

        if (animator && resetMovementParamsOnDeath)
        {
            if (hasMovementXParam)
            {
                animator.SetFloat(movementXParam, 0f);
            }

            if (hasMovementYParam)
            {
                animator.SetFloat(movementYParam, 0f);
            }
        }
    }

    private void DisableCombat()
    {
        EnemyAI enemyAI = GetComponentInParent<EnemyAI>();
        if (!enemyAI)
        {
            enemyAI = GetComponentInChildren<EnemyAI>();
        }

        if (enemyAI)
        {
            enemyAI.CancelScheduledFireball();
            enemyAI.enabled = false;
        }

        EnemyAttack enemyAttack = GetComponentInParent<EnemyAttack>();
        if (!enemyAttack)
        {
            enemyAttack = GetComponentInChildren<EnemyAttack>();
        }

        if (enemyAttack)
        {
            enemyAttack.enabled = false;
        }

        ComboExecutor[] executors = GetComponentsInChildren<ComboExecutor>(true);
        for (int i = 0; i < executors.Length; i++)
        {
            executors[i].enabled = false;
        }
    }

    private void CacheAnimatorParams()
    {
        if (!animator)
        {
            return;
        }

        AnimatorControllerParameter[] parameters = animator.parameters;
        for (int i = 0; i < parameters.Length; i++)
        {
            string name = parameters[i].name;
            if (!hasMovementXParam && name == movementXParam)
            {
                hasMovementXParam = true;
            }

            if (!hasMovementYParam && name == movementYParam)
            {
                hasMovementYParam = true;
            }
        }
    }
}
