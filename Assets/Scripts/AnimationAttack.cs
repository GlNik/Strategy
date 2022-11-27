using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    [SerializeField] private Viking _viking;

    public void Attack()
    {
        if (_viking != null)
        {
            if (_viking.CurrentUnitState == UnitState.Attack)
            {
                _viking.AttackFromAnimation();
            }
        }
        if (_viking.CurrentUnitState == UnitState.AttackBuilding)
        {
            _viking.AttackBuildingFromAnimation();
        }
    }
}
