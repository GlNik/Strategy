using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack,
    WalkToEnemyBuilding,
    AttackBuilding,
    //worker state
    Work,
    WalkToWorkBuilding
}

public class Unit : SelectableObject
{
    public int Price;
    public NavMeshAgent NavMeshAgent;
    [SerializeField] private int _health = 5;
    private int _maxHealth;
    public Animator Animator;

    public virtual new void Start()
    {
        _maxHealth = _health;
    }

    public virtual void SetTarget(SelectableObject target)
    {

    }

    public virtual void Update()
    {
        if (NavMeshAgent.velocity.magnitude >= 0.05f)
        {
            Animator.SetBool("InMotion", true);
        }
        else
        {
            Animator.SetBool("InMotion", false);
        }
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);

        NavMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        if (_health <= 0)
        {
            Die();
        }
        HealthBar.SetHealth(_health, _maxHealth);
    }

    private void Die()
    {
        Destroy(this);
        Destroy(NavMeshAgent);
        Unselect();
        Animator.SetTrigger("Die");
        Destroy(gameObject, 4f);
    }

    private void OnDestroy()
    {
        if (Management.Instance)
            Management.Instance.Unselect(this);
    }
}