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
        if (_enemy.CurrentEnemyState == EnemyState.AttackBuilding)
        {
            _enemy.AttackBuildingFromAnimation();
        }
    }
}
