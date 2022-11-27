using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class Worker : Unit
{    
    public UnitState CurrentWorkerState;

    private Vector3 _targetPoint;
    private Enemy _targetEnemy;
    private EnemyBuilding _targetBuilding;
    [SerializeField] private float _distanceToFollow = 7f;
    [SerializeField] private float _distanceToAttack = 1.5f;
    private float _distance;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _stopAround = 1.5f;
    [SerializeField] private GameObject _pointClickFX;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.Idle);
        UnitsManager.Instance.AddWorker(this);
       // StartCoroutine(FindTarget());
    }

    public override void SetTarget(SelectableObject target)
    {
        if (target.TryGetComponent(out Enemy enemy))
        {
            _targetEnemy = enemy;
            //SetState(WorkerState.WalkToEnemy);
        }
        else if (target.TryGetComponent(out EnemyBuilding enemyBuilding))
        {
            _targetBuilding = enemyBuilding;
            SetState(UnitState.WalkToWorkBuilding);
        }
    }

    public override void Update()
    {
        base.Update();

        cntFramesAfterSetState++;

        //if (CurrentWorkerState == UnitState.Attack || CurrentWorkerState == UnitState.AttackBuilding)
        //{
        //    Animator.SetBool("Attacking", true);
        //}
        //else
        //{
        //    Animator.SetBool("Attacking", false);
        //}

        switch (CurrentWorkerState)
        {
            case UnitState.Idle:
                Idle();
                break;
            case UnitState.WalkToPoint:
                WalkToPoint();
                break;
            case UnitState.WalkToWorkBuilding:
                WalkToBuilding();
                break;
        }
    }

    private void Idle()
    {
        if ((NavMeshAgent.destination - transform.position).magnitude < 0.9f)
            NavMeshAgent.SetDestination(transform.position);
        Animator.SetBool("InMotion", false);
    }

    private void WalkToPoint()
    {
        if (cntFramesAfterSetState > 2)
            // обезопашиваемся от нуля
            if (NavMeshAgent.remainingDistance < Mathf.Clamp(NavMeshAgent.stoppingDistance, 0.01f, float.MaxValue))
            {
                if (NavMeshAgent.velocity.magnitude <= 0.01f)
                {
                    SetState(UnitState.Idle);
                }
            }
    }

    //private void WalkToEnemy()
    //{
    //    if (!_targetEnemy)
    //    {
    //        SetState(WorkerState.Idle);
    //    }
    //    else
    //    {
    //        _distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
    //        if (_distance > _distanceToFollow)
    //        {
    //            SetState(WorkerState.Idle);
    //        }
    //        else
    //        {
    //            NavMeshAgent.SetDestination(_targetEnemy.transform.position);
    //            if (_distance < _distanceToAttack)
    //            {
    //                //SetState(WorkerState.Attack);
    //            }
    //        }
    //    }
    //}

    private void WalkToBuilding()
    {
        if (_targetBuilding == null)
        {
            SetState(UnitState.Idle);
            NavMeshAgent.SetDestination(transform.position);
        }
        else
        {
            // как избежать GetComponentInChildren ?
            _distance = Vector3.Distance(transform.position, _targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
            if (_distance < _distanceToAttack)
            {
                // SetState(WorkerState.AttackBuilding);
            }
        }
    }

    //private void Attack()
    //{
    //    if (!_targetEnemy)
    //    {
    //        SetState(WorkerState.Idle);
    //    }
    //    else
    //    {
    //        FaceTarget(_targetEnemy.transform.position);
    //        _distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
    //        if (_distance > _distanceToAttack)
    //        {
    //            // SetState(WorkerState.WalkToEnemy);
    //        }
    //    }
    //}

    //public void AttackFromAnimation()
    //{
    //    if (_targetEnemy)
    //        _targetEnemy.TakeDamage(_damage);
    //}

    //private void AttackBuilding()
    //{
    //    if (!_targetBuilding)
    //    {
    //        SetState(WorkerState.Idle);
    //    }
    //    else
    //    {
    //        // как избежать GetComponentInChildren ?
    //        FaceTarget(_targetBuilding.transform.position);
    //        _distance = Vector3.Distance(transform.position, _targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
    //        if (_distance > _distanceToAttack)
    //        {
    //            SetState(WorkerState.WalkToBuilding);
    //        }
    //    }
    //}

    //public void AttackBuildingFromAnimation()
    //{
    //    if (_targetBuilding)
    //        _targetBuilding.TakeDamage(_damage);
    //}

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
        CurrentWorkerState = UnitState;

        switch (CurrentWorkerState)
        {
            //case UnitState.Idle:                
            //    break;
            case UnitState.WalkToPoint:
                NavMeshAgent.stoppingDistance = 0.05f;                
                break;
            case UnitState.WalkToWorkBuilding:
                if (_targetBuilding)
                {
                    NavMeshAgent.SetDestination(_targetBuilding.GetComponentInChildren<Collider>().bounds.ClosestPoint(transform.position));
                }
                break;

        }
    }

    private void OnDestroy()
    {
         UnitsManager.Instance.RemoveWorker(this);
    }

    //private bool FindClosestEnemy()
    //{
    //    _targetEnemy = UnitsManager.Instance.GetClousestEnemy(transform.position, _distanceToFollow);
    //    return _targetEnemy != null;
    //}

    //private IEnumerator FindTarget()
    //{
    //    while (true)
    //    {
    //        if (CurrentWorkerState == WorkerState.Idle)
    //        {
    //            if (FindClosestEnemy())
    //            {
    //                // SetState(WorkerState.WalkToEnemy);
    //            }
    //        }
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        SetState(UnitState.WalkToPoint);
        NavMeshAgent.SetDestination(point);
        Instantiate(_pointClickFX, point + new Vector3(0, 0.1f, 0), Quaternion.identity);
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
