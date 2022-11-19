using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack,
    AttackBuilding
}

public class Enemy : SelectableObject
{
    public EnemyState CurrentEnemyState;

    [SerializeField] int _health = 5;
    Building _targetBuilding;
    Unit _targetUnit;
    [SerializeField] float _distanceToFollow = 7f;
    [SerializeField] float _distanceToAttack = 1.5f;

    [SerializeField] NavMeshAgent _navMeshAgent;

    //[SerializeField] float _attackPeriod = 1f;
    //private float _timer;

    float _distance;

    private int _maxHealth;

    [SerializeField] Animator _animator;

    private bool _isGuard = false;
    [SerializeField] int _damage = 1;

    void Start()
    {
        _maxHealth = _health;
        if (_isGuard)
            SetState(EnemyState.Idle);
        else
            SetState(EnemyState.WalkToBuilding);

        UnitsManager.Instance.AddEnemy(this);
    }

    void Update()
    {
        if (_navMeshAgent.velocity.magnitude >= 0.05f)
        {
            _animator.SetBool("InMotion", true);
        }
        else
        {
            _animator.SetBool("InMotion", false);
        }

        if (CurrentEnemyState == EnemyState.Attack || CurrentEnemyState == EnemyState.AttackBuilding)
        {
            _animator.SetBool("Attacking", true);
        }
        else
        {
            _animator.SetBool("Attacking", false);
        }

        switch (CurrentEnemyState)
        {
            case EnemyState.Idle:
                Idle();
                _animator.SetBool("InMotion", false);
                break;
            case EnemyState.WalkToBuilding:
                WalkToBuilding();
                break;
            case EnemyState.WalkToUnit:
                WalkToUnit();
                FindTarget();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.AttackBuilding:
                AttackBuilding();
                break;
        }
    }

    private void Idle()
    {
        if (FindClosestBuilding())
        {
            if (!_isGuard)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            _animator.SetBool("InMotion", false);
        }
        else if (FindClosestUnit())
        {
            SetState(EnemyState.WalkToUnit);
        }
        else
        {
            SetState(EnemyState.Idle);
            _animator.SetBool("InMotion", false);
        }
    }

    private void WalkToBuilding()
    {
        FindClosestUnit();
        if (_targetBuilding == null)
        {
            SetState(EnemyState.Idle);
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetBool("InMotion", false);
        }
        else
        {
            // как избежать GetComponentInChildren ?
            _distance = Vector3.Distance(transform.position, _targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
            if (_distance < _distanceToAttack)
                SetState(EnemyState.AttackBuilding);
        }
    }

    private void WalkToUnit()
    {
        if (!_targetUnit)
        {
            if (_isGuard)
            {
                SetState(EnemyState.Idle);
            }
            else
                SetState(EnemyState.WalkToBuilding);
        }
        else
        {
            _navMeshAgent.SetDestination(_targetUnit.transform.position);
            _distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
            if (_distance > _distanceToFollow)
            {
                if (_isGuard)
                    SetState(EnemyState.Idle);
                else
                    SetState(EnemyState.WalkToBuilding);
            }
            if (_distance < _distanceToAttack)
                SetState(EnemyState.Attack);
        }
    }

    private void Attack()
    {
        if (!_targetUnit)
        {
            if (_isGuard)
                SetState(EnemyState.Idle);
            else
                SetState(EnemyState.WalkToBuilding);

        }
        else
        {
            FaceTarget(_targetUnit.transform.position);
            _distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
            if (_distance > _distanceToAttack)
                SetState(EnemyState.WalkToUnit);
        }
    }

    public void AttackFromAnimation()
    {
        if (_targetUnit)
            _targetUnit.TakeDamage(_damage);
    }

    private void AttackBuilding()
    {
        if (!_targetBuilding)
        {
            SetState(EnemyState.WalkToBuilding);
        }
        else
        {
            FaceTarget(_targetBuilding.transform.position);
            // как избежать GetComponentInChildren ?
            _distance = Vector3.Distance(transform.position, _targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
            if (_distance > _distanceToAttack)
                SetState(EnemyState.WalkToBuilding);
        }
    }
    public void AttackBuildingFromAnimation()
    {
        if (_targetBuilding)
            _targetBuilding.TakeDamage(_damage);
    }

    private void FaceTarget(Vector3 destination) // for stopping distance
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100f * Time.deltaTime);
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;

        switch (CurrentEnemyState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.WalkToBuilding:
                if (_isGuard)
                {
                    SetState(EnemyState.Idle);
                    break;
                }
                if (_targetBuilding)
                {
                    // как избежать GetComponentInChildren ?
                    _navMeshAgent.SetDestination(_targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
                }
                break;
            case EnemyState.WalkToUnit:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.AttackBuilding:
                break;
        }
    }

    IEnumerator FindTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            FindClosestUnit();
            // yield return null;
        }
    }

    private bool FindClosestBuilding()
    {
        _targetBuilding = BuildingPlacer.Instance.GetClousestBuilding(transform.position);
        return _targetBuilding != null;
    }

    private bool FindClosestUnit()
    {
        _targetUnit = UnitsManager.Instance.GetClousestUnit(transform.position);
        return _targetUnit != null;
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

    void Die()
    {
        Destroy(this);
        Destroy(_navMeshAgent);
        //Animator.SetBool("InMotion", false);
        // Animator.SetBool("Attacking", false);
        _animator.SetTrigger("Die");
        Destroy(gameObject, 4f);
    }

    private void OnDestroy()
    {
        UnitsManager.Instance.RemoveEnemy(this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif
}
