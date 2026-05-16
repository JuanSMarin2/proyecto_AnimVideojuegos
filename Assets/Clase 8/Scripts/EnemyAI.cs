using Clases.Clase_8.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private State currentState;
public NavMeshAgent agent;
public Transform player;
public Transform playerTarget;

public Animator animator;

[Header("Movement")]
public float walkSpeed = 2.0f;
public float runSpeed = 3.5f;
public float rotationSmooth = 12.0f;
public float animationSmooth = 10.0f;

private Vector3 patrolPointA;
private Vector3 patrolPointB;
private bool hasPatrolPoints;

public ComboSequence defaultCombo;

[Header("Escape")]
[SerializeField] private int hitsToEscape = 2;
[SerializeField] private float escapeDuration = 1f;
[SerializeField] private float escapeDistance = 4f;

[Header("Combat")]
[SerializeField] private bool isMage;
[SerializeField] private bool isMageBoss;
[SerializeField] private bool isFinalBoss;
[SerializeField] private float shootRange = 8f;
[SerializeField] private GameObject fireBallPrefab;
[SerializeField] private Transform fireBallSpawn;
[SerializeField] private float fireBallSpawnHeightOffset = 0f;
[SerializeField] private float fireBallForce = 12f;
[SerializeField] private float shootCooldown = 3f;
[SerializeField] private float bossSpreadAngle = 20f;
[SerializeField] private float finalBossSpreadAngle = 20f;
[SerializeField] private float shootPitchOffset = 10f;

[Header("Mage Shoot Timing")]
[SerializeField] private float shootDelay = 0.5f;

[Header("Targeting")]
[SerializeField] private string playerTag = "Player";
[SerializeField] private string playerTargetTag = "PlayerTarget";

public bool IsMage => isMage;
public bool IsMageBoss => isMageBoss;
public bool IsFinalBoss => isFinalBoss;
public float ShootRange => shootRange;
public GameObject FireBallPrefab => fireBallPrefab;
public Transform FireBallSpawn => fireBallSpawn;
public float FireBallSpawnHeightOffset => fireBallSpawnHeightOffset;
public float FireBallForce => fireBallForce;
public float ShootCooldown => shootCooldown;
public float ShootDelay => shootDelay;
public float BossSpreadAngle => bossSpreadAngle;
public float FinalBossSpreadAngle => finalBossSpreadAngle;
public float ShootPitchOffset => shootPitchOffset;

private Coroutine shootRoutine;
private int consecutiveHitsWithoutAttack;
private bool attackedSinceLastHit;

static class Hash
    {
        public static readonly int SpeedX = Animator.StringToHash("SpeedX");
        public static readonly int SpeedY = Animator.StringToHash("SpeedY");
    }

private void Start()
    {
        ResolveTargets();

        agent.updatePosition = true;
        agent.updateRotation = false;
        animator.applyRootMotion = false;

        ChangeState(new IdleState(this));
    }

private void ResolveTargets()
    {
        if (!player)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObject)
            {
                player = playerObject.transform;
            }
        }

        if (!playerTarget)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag(playerTargetTag);
            if (targetObject)
            {
                playerTarget = targetObject.transform;
            }
        }

        if (!playerTarget && player)
        {
            playerTarget = player;
        }
    }

private void Update()
{
    currentState?.Update();

    Vector3 desird = agent.desiredVelocity;
    desird.y = 0;

    if (desird.sqrMagnitude > 0.001f)
    {
        Quaternion look = Quaternion.LookRotation(desird, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSmooth * Time.deltaTime);
    }

    Vector3 dirLocal = desird.sqrMagnitude > 0.001f ? transform.InverseTransformDirection(desird.normalized) : Vector3.zero;

    float denom = Mathf.Max(0.01f, agent.speed);
    float mag01 = Mathf.Clamp01(agent.velocity.magnitude / denom);

    float targetX = dirLocal.x * mag01;
    float targetY = dirLocal.z * mag01; 

    float curX = Mathf.Lerp(animator.GetFloat(Hash.SpeedX), targetX, Time.deltaTime * animationSmooth);
    float curY = Mathf.Lerp(animator.GetFloat(Hash.SpeedY), targetY, Time.deltaTime * animationSmooth);

    animator.SetFloat(Hash.SpeedX, curX);
    animator.SetFloat(Hash.SpeedY, curY);
}

    public void ChangeState(State newState)
    {
currentState?.Exit();
currentState = newState;
currentState?.Enter();
        
    }

    public void NotifyHit()
        {
            if (currentState is Clases.Clase_8.Scripts.States.ScapeState)
            {
                return;
            }

            if (attackedSinceLastHit)
            {
                consecutiveHitsWithoutAttack = 1;
                attackedSinceLastHit = false;
            }
            else
            {
                consecutiveHitsWithoutAttack++;
            }

            if (consecutiveHitsWithoutAttack >= Mathf.Max(1, hitsToEscape))
            {
                consecutiveHitsWithoutAttack = 0;
                ChangeState(new Clases.Clase_8.Scripts.States.ScapeState(this, escapeDuration, escapeDistance));
            }
        }

    public void NotifyAttack()
        {
            attackedSinceLastHit = true;
            consecutiveHitsWithoutAttack = 0;
        }

public void NextWayPoint()
    {
        if (!agent)
        {
            return;
        }

        if (!hasPatrolPoints)
        {
            hasPatrolPoints = TryPickPatrolPoints(out patrolPointA, out patrolPointB);
        }

        if (!hasPatrolPoints)
        {
            return;
        }

        Vector3 target = Vector3.Distance(transform.position, patrolPointA) <= Vector3.Distance(transform.position, patrolPointB)
            ? patrolPointB
            : patrolPointA;

        agent.SetDestination(target);
    }

private bool TryPickPatrolPoints(out Vector3 pointA, out Vector3 pointB)
    {
        pointA = transform.position;
        pointB = transform.position;

        if (!agent || !agent.isOnNavMesh)
        {
            return false;
        }

        const float searchRadius = 10f;
        if (!UnityEngine.AI.NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * searchRadius, out var hitA, searchRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            return false;
        }

        if (!UnityEngine.AI.NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * searchRadius, out var hitB, searchRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            return false;
        }

        pointA = hitA.position;
        pointB = hitB.position;
        return true;
    }


public bool PlayerInRange(float range)
    {
        if(player == null) return false;
        if (!playerTarget)
        {
            return false;
        }

        return Vector3.Distance(transform.position, playerTarget.position) < range;
    }

public bool PlayerInShootRange()
    {
        return PlayerInRange(shootRange);
    }

public void ScheduleFireball(float delay)
    {
        if (shootRoutine != null)
        {
            StopCoroutine(shootRoutine);
        }

        shootRoutine = StartCoroutine(ShootAfterDelay(Mathf.Max(0f, delay)));
    }

public void CancelScheduledFireball()
    {
        if (shootRoutine == null)
        {
            return;
        }

        StopCoroutine(shootRoutine);
        shootRoutine = null;
    }

private System.Collections.IEnumerator ShootAfterDelay(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        shootRoutine = null;
        SpawnFireball();
    }

public void SpawnFireball()
    {
        if (!fireBallPrefab || playerTarget == null)
        {
            return;
        }

        Vector3 origin = fireBallSpawn ? fireBallSpawn.position : transform.position + Vector3.up * fireBallSpawnHeightOffset;
        Vector3 direction = (playerTarget.position - origin).normalized;
        if (isFinalBoss)
        {
            SpawnFinalBossVolley(origin, direction);
            return;
        }

        if (isMageBoss)
        {
            SpawnFireballInstance(origin, direction);
            SpawnFireballInstance(origin, Quaternion.AngleAxis(-bossSpreadAngle, Vector3.up) * direction);
            SpawnFireballInstance(origin, Quaternion.AngleAxis(bossSpreadAngle, Vector3.up) * direction);
            return;
        }

        SpawnFireballInstance(origin, direction);
    }

private void SpawnFireballInstance(Vector3 origin, Vector3 direction)
    {
        Vector3 adjustedDirection = ApplyPitchOffset(direction);
        GameObject instance = Instantiate(fireBallPrefab, origin, Quaternion.LookRotation(adjustedDirection));

        FireBall fireBall = instance.GetComponent<FireBall>();
        if (fireBall)
        {
            fireBall.Launch(adjustedDirection, fireBallForce);
            return;
        }

        Rigidbody rb = instance.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(adjustedDirection * fireBallForce, ForceMode.VelocityChange);
        }
    }

private void SpawnFinalBossVolley(Vector3 origin, Vector3 direction)
    {
        float spread = Mathf.Max(0f, finalBossSpreadAngle);
        for (int i = -2; i <= 2; i++)
        {
            Vector3 yawDir = Quaternion.AngleAxis(spread * i, Vector3.up) * direction;
            SpawnFireballInstance(origin, yawDir);
            SpawnFireballInstance(origin, -yawDir);
        }
    }

private Vector3 ApplyPitchOffset(Vector3 direction)
    {
        float pitch = shootPitchOffset;
        if (Mathf.Abs(pitch) < 0.001f)
        {
            return direction;
        }

        Vector3 right = Vector3.Cross(Vector3.up, direction).normalized;
        if (right.sqrMagnitude < 0.0001f)
        {
            return direction;
        }

        return Quaternion.AngleAxis(pitch, right) * direction;
    }
}
