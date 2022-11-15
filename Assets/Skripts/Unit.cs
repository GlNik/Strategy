using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    public int Price;
    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
    }
}
