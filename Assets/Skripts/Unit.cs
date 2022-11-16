using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    public int Price;
    [SerializeField] int _health;
    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
