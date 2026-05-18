using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float minLifeBeforeDestroy = 0.5f;
    [SerializeField] private float force = 12f;
    [SerializeField] private bool useUnscaledLifetime = true;
    [SerializeField] private string obstacleTag = "Obstacle";
  

    private Rigidbody rb;
    private float spawnTime;
    private bool destroyScheduled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        spawnTime = GetTimeNow();
        ScheduleDestroy();
    }

    private void Start()
    {
        ScheduleDestroy();
    }

    public void Launch(Vector3 direction, float forceOverride)
    {
        if (rb)
        {
            float finalForce = forceOverride > 0f ? forceOverride : force;
            rb.AddForce(direction * finalForce, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitPlayer = ApplyDamage(other);
        bool shouldDestroy = hitPlayer
            || other.CompareTag(obstacleTag)
     ;

        if (shouldDestroy)
        {
            TryDestroyOnContact();
        }
    }

    private void ScheduleDestroy()
    {
        if (lifeTime <= 0f)
        {
            return;
        }

        if (useUnscaledLifetime)
        {
            StartCoroutine(DestroyAfterRealtime(lifeTime));
            return;
        }

        Destroy(gameObject, lifeTime);
    }

    private System.Collections.IEnumerator DestroyAfterRealtime(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (this)
        {
            Destroy(gameObject);
        }
    }

    private void TryDestroyOnContact()
    {
        if (destroyScheduled)
        {
            return;
        }

        float age = GetTimeNow() - spawnTime;
        if (age >= minLifeBeforeDestroy)
        {
            Destroy(gameObject);
            return;
        }

        destroyScheduled = true;
        float remaining = Mathf.Max(0f, minLifeBeforeDestroy - age);
        if (useUnscaledLifetime)
        {
            StartCoroutine(DestroyAfterRealtime(remaining));
        }
        else
        {
            Destroy(gameObject, remaining);
        }
    }

    private float GetTimeNow()
    {
        return useUnscaledLifetime ? Time.unscaledTime : Time.time;
    }

    private bool ApplyDamage(Collider other)
    {
        DirectionalHitbox hitbox = other.GetComponent<DirectionalHitbox>();
        if (!hitbox)
        {
            hitbox = other.GetComponentInParent<DirectionalHitbox>();
        }

        if (!hitbox)

        {
            return false;
        }

        hitbox.ReceiveProjectile(damage, gameObject);
        return true;
    }
}
