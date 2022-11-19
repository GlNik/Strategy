using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Knight : Unit
{
    public UnitState CurrentUnitState;

    private Vector3 _targetPoint;
    private Enemy _targetEnemy;
    private EnemyBuilding _targetBuilding;
    [SerializeField] float _distanceToFollow = 7f;
    [SerializeField] float _distanceToAttack = 1.5f;

   // [SerializeField] float _attackPeriod = 1f;
   // private float _timer;

    float _distance;
    [SerializeField] int _damage = 1;
    [SerializeField] float _stopAround = 1.5f;
    private Coroutine _coroutine;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.Idle);
        UnitsManager.Instance.AddKnight(this);
    }

    public override void SetTarget(SelectableObject target)
    {
        if (target.TryGetComponent(out Enemy enemy))
        {
            _targetEnemy = enemy;
            SetState(UnitState.WalkToEnemy);
        }
        else if (target.TryGetComponent(out EnemyBuilding enemyBuilding))
        {
            _targetBuilding = enemyBuilding;
            SetState(UnitState.WalkToEnemyBuilding);
        }
    }

    public override void Update()
    {
        base.Update();

        cntFramesAfterSetState++;

        if (CurrentUnitState == UnitState.Attack || CurrentUnitState == UnitState.AttackBuilding)
        {
            Animator.SetBool("Attacking", true);
        }
        else
        {
            Animator.SetBool("Attacking", false);
        }

        switch (CurrentUnitState)
        {
            case UnitState.Idle:
                Idle();
                break;
            case UnitState.WalkToPoint:
                WalkToPoint();
                break;
            case UnitState.WalkToEnemy:
                WalkToEnemy();
                break;
            case UnitState.WalkToEnemyBuilding:
                WalkToEnemyBuilding();
                break;
            case UnitState.Attack:
                Attack();
                break;
            case UnitState.AttackBuilding:
                AttackBuilding();
                break;
        }
    }

    private void Idle()
    {
        if ((NavMeshAgent.destination - transform.position).magnitude < 0.9f)
            NavMeshAgent.SetDestination(transform.position);
        if (FindClosestEnemy())
        {
            SetState(UnitState.WalkToEnemy);
        }
    }

    private void WalkToPoint()
    {
        if (cntFramesAfterSetState > 2)
            if (NavMeshAgent.remainingDistance < Mathf.Clamp(NavMeshAgent.stoppingDistance, 0.01f, float.MaxValue)) // ��������������� �� ����
                SetState(UnitState.Idle);
    }

    private void WalkToEnemy()
    {
        if (!_targetEnemy)
        {
            SetState(UnitState.Idle);
            //break;          
        }
        else
        {
            NavMeshAgent.SetDestination(_targetEnemy.transform.position);
            _distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
            if (_distance > _distanceToFollow)
                SetState(UnitState.Idle);
            if (_distance < _distanceToAttack)
                SetState(UnitState.Attack);
        }
    }

    private void WalkToEnemyBuilding()
    {
        if (_targetBuilding == null)
        {
            SetState(UnitState.Idle);
            NavMeshAgent.SetDestination(transform.position);
            //break;
        }
        else
        {
            // ��� �������� GetComponentInChildren ?
            _distance = Vector3.Distance(transform.position, _targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
            if (_distance < _distanceToAttack)
                SetState(UnitState.AttackBuilding);
        }
    }

    private void Attack()
    {
        if (!_targetEnemy)
        {
            SetState(UnitState.Idle);
            //break;
        }
        else
        {
            FaceTarget(_targetEnemy.transform.position);
            _distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
            if (_distance > _distanceToAttack)
                SetState(UnitState.WalkToEnemy);



            //StartCoroutine(FindTarget());
        }
    }


    public void AttackFromAnimation()
    {
        if (_targetEnemy)
            _targetEnemy.TakeDamage(_damage);
    }

    private void AttackBuilding()
    {
        if (!_targetBuilding)
        {
            SetState(UnitState.Idle);
            // break;
        }
        else
        {
            FaceTarget(_targetBuilding.transform.position);
            // ��� �������� GetComponentInChildren ?
            _distance = Vector3.Distance(transform.position, _targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
            if (_distance > _distanceToAttack)
                SetState(UnitState.WalkToEnemyBuilding);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);
    }

    private int cntFramesAfterSetState = 0;

    public void SetState(UnitState UnitState)
    {

        cntFramesAfterSetState = 0;

        CurrentUnitState = UnitState;


        switch (CurrentUnitState)
        {
            case UnitState.Idle:
                break;
            case UnitState.WalkToPoint:
                NavMeshAgent.stoppingDistance = 0.05f;
                break;
            case UnitState.WalkToEnemy:
                NavMeshAgent.stoppingDistance = _stopAround;
                break;
            case UnitState.Attack:
                NavMeshAgent.stoppingDistance = _stopAround;
                break;
            case UnitState.WalkToEnemyBuilding:
                if (_targetBuilding)
                {
                    // ��� �������� GetComponentInChildren ?
                    NavMeshAgent.SetDestination(_targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
                }
                break;
            case UnitState.AttackBuilding:
                break;
        }
    }

    private void OnDestroy()
    {
        UnitsManager.Instance.RemoveKnight(this);
    }
    private bool FindClosestEnemy()
    {
        _targetEnemy = UnitsManager.Instance.GetClousestEnemy(transform.position, _distanceToFollow);
        return _targetEnemy != null;
    }

    // ��� ����� ������ �������������� ����� � �������� ����?
    // ���� ��� � ����� �� ����� � �����, �� ������ ����� ��� ���� ����������, � �� ������ ���� ������� ����� �� ������, 
    // ���� ����� ���� �� ������ ������� ����� ����� ���� � ������ �����
    //IEnumerator FindTarget()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        FindClosestEnemy();
    //        SetState(UnitState.WalkToEnemy);
    //        print("work");
    //        // yield return null;
    //    }
    //}

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        SetState(UnitState.WalkToPoint);
        NavMeshAgent.SetDestination(point);

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
