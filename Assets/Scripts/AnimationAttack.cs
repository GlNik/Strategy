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
            _viking.AttackFromAnimation();
        }
        _viking.AttackBuildingFromAnimation();
    }
}
