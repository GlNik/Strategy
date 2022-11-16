using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;
    [SerializeField] int _health;
    [SerializeField] Building _targetBuilding;
    [SerializeField] Unit _targetUnit;
    [SerializeField] float _distanceToAttack = 1f;
    [SerializeField] float _distanceToFollow = 7f;
    [SerializeField] NavMeshAgent _navMeshAgent;

    [SerializeField] float _attaclPeriod = 1f;
    float _timer;

    [SerializeField] int _damage = 1;

    private void Start()
    {
        SetState(EnemyState.WalkToBuilding);
    }
    void Update()
    {
        if (CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestUnit();
        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestUnit();
            if (_targetBuilding == null)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > _distanceToFollow)
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                if (distance < _distanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            if (_targetUnit)
            {
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > _distanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer = Time.deltaTime;
                if (_timer > _attaclPeriod)
                {
                    _timer = 0;
                    _targetUnit.TakeDamage(_damage);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle)
        {

        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestBuilding();
            _navMeshAgent.SetDestination(_targetBuilding.transform.position);
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosestBuilding()
    {
        Building[] allBuildings = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        for (int i = 0; i < allBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }
        _targetBuilding = closestBuilding;
    }

    public void FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }
        if (minDistance < _distanceToFollow)
        {
            _targetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
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
