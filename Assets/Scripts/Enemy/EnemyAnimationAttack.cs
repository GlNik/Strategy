using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public void Attack()
    {
        if (_enemy != null)
        {
            if (_enemy.CurrentEnemyState == EnemyState.Attack)
            {
                _enemy.AttackFromAnimation();
            }
        }
        _enemy.AttackBuildingFromAnimation();
    }
}
