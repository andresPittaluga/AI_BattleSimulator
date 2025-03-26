using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boid : SteeringAgent, IDamageable
{
    [Header("<color=#FBAF00>DebugReferences")]
    public Material teamMat;
    [SerializeField] protected Color _gizmoColor;
    protected string _hexColor;

    [Header("<color=#11AA11>Stats")]
    [SerializeField] protected int _startHp;
    public float attackCD = 1.5f;
    public float healTime = 1.5f;
    protected int _actualHp;

    [Header("<color=#11CAC1>AreaValues")]

    public float evadeDistence;
    public float pursuitDistance;
    public float healDistance;

    [Header("<color=#332f98>References")]
    public LayerMask enemyMask;
    [SerializeField] protected TypeEvent _team;
    [SerializeField] protected float _viewAngle = 90f;
    [SerializeField] private Image _healthBar;
    [SerializeField] public CustomParticles particles;
    public BulletFactory BulletFactory;

    [Header("<color=#9b4ebd>FlockStats")]
    [SerializeField] protected float _separationRadius = 4;
    [SerializeField, Range(0f, 3f)] protected float _separationWeight = 1f;
    [Range(0f, 3f)] public float evadeWeight = 0.25f;
    [Range(0f, 3f)] public float obstacleAvoidanceWeight = 1f;
    protected BoidManager BM => BoidManager.Instance;
    protected FSM _fsm;

    protected Rest _rest;
    protected Attack _attack;
    protected Retreat _retreat;

    protected const string PATH_ACHIEVED = "REST";
    protected const string ENEMY_SPOTTED = "ATTACK";
    protected const string ENEMY_LOST = "WAIT_FOR_COMMAND";
    protected const string LOW_HEALTH = "RETREAT";
    protected const string MOVE_COMMAND = "LEAD";

    protected virtual void Start()
    {
        BM.AddBoid(this);
        _hexColor = Observe.ToHex(_gizmoColor);

        _actualHp = _startHp;

        CreateFSM();
    }
    protected virtual void Update()
    {
        if (AreEnemiesOnFOV() && _fsm.GetState() != _retreat)
            EnemySpotted();
    }
    public void PrettyPrint(string msg)
    {
        Debug.Log($"<color={_hexColor}>{gameObject.name}:</color> {msg}");
    }
    private void OnDrawGizmos()
    {
        Observe.DrawFieldOfViewGizmo(transform.position, transform.forward, _viewAngle, _viewRadius, _gizmoColor);
    }
    protected virtual void OnDestroy()
    {
        BM.RemoveBoid(this);
        GameManager.Instance.Eliminate(_team, transform);
    }
    #region FSM
    private void CreateFSM()
    {
        _rest = new Rest(this);

        _fsm = new FSM(_rest);
    }
    public abstract void LostTrack();
    public abstract void MoveAgain(Pf_Node node);
    public void EnemyLost() => _fsm.SendInput(ENEMY_LOST);
    public void EnemySpotted() => _fsm.SendInput(ENEMY_SPOTTED);
    public void PathAchieved() => _fsm.SendInput(PATH_ACHIEVED);
    public void Retreat() => _fsm.SendInput(LOW_HEALTH);
    #endregion

    #region Battle Methods
    public bool AreEnemiesOnFOV()
    {
        foreach (Transform enemy in GameManager.Instance.GetEnemyTeam(_team))
        {
            if (Observe.FieldOfView(transform, enemy, _viewAngle, _viewRadius, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }
    public bool AreEnemiesOnLOS(out Transform enemyTransform) // Utilizado como auxiliar en AreEnemiesInArea
    {
        foreach (Transform enemy in GameManager.Instance.GetEnemyTeam(_team))
        {
            if (Observe.LineOfSight(transform.position, enemy.position, obstacleMask))
            {
                enemyTransform = enemy;
                return true;
            }
        }
        enemyTransform = null;
        return false;
    }
    public bool AreEnemiesInArea(float area) // Usado para ver si hay que seguir en modo ataque
    {
        if (AreEnemiesOnLOS(out Transform enemy))
        {
            return (enemy.position - transform.position).sqrMagnitude < (_viewRadius * _viewRadius) * area;
        }
        return false;
    }
    public bool AreEnemiesInArea(out Transform Enemy, float area) // Usado para ver si hay que seguir en modo ataque (Sobrecarga)
    {
        if (AreEnemiesOnLOS(out Transform enemy))
        {
            Enemy = enemy;
            return (enemy.position - transform.position).sqrMagnitude < (_viewRadius * _viewRadius) * area;
        }
        Enemy = null;
        return false;
    }
    public SteeringAgent GetClosestEnemyAgent(float radiusMult = 2) // Utilizado para hacer evade/pursuit en modo ataque
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _viewRadius * radiusMult, enemyMask);

        if (colls.Length > 0)
        {
            if (colls[0].TryGetComponent<SteeringAgent>(out SteeringAgent enemy))
            {
                return enemy;
            }
            else return null;
        }
        else return null;
    }
    public Transform GetExactClosestEnemy() // Encuentra el enemigo más cercano
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in GameManager.Instance.GetEnemyTeam(_team))
        {
            float distance = (enemy.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
    public void TakeDamage(int damage)
    {
        particles.PlayDmgParticles();
        _actualHp -= damage;

        RefreshHealthBar();

        if (_actualHp <= _startHp * .5f)
            Retreat();

        if (_actualHp <= 0)
            Die();
    }

    public void Heal(int healthAmount)
    {
        if (_actualHp >= _startHp)
        {
            MoveAgain(null);
            return;
        }

        particles.PlayHealthParticles();
        _actualHp += healthAmount;

        if (_actualHp >= _startHp)
            MoveAgain(null);

        RefreshHealthBar();
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    private void RefreshHealthBar() => _healthBar.fillAmount = (float)_actualHp / _startHp;
    #endregion

    #region Flock Methods
    public Vector3 Arriving(Transform t, float weight) => Arrive(t.position) * weight;
    public Vector3 Separate() => Separation(_separationRadius) * _separationWeight;
    protected Vector3 Separation(float radius)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var item in BM.AllBoids)
        {
            if (item == this) continue;
            Vector3 dir = item.transform.position - transform.position;
            if (dir.sqrMagnitude > radius * radius) continue;
            count++;
            desired += dir;
        }
        if (count == 0) return desired;
        desired /= count;
        desired *= -1;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }
    #endregion
}
