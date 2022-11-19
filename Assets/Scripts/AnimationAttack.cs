using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAttack : MonoBehaviour
{
    [SerializeField] private Knight _knight;

    public void Attack()
    {
        if (_knight != null)
        {
            _knight.AttackFromAnimation();
        }
        _knight.AttackBuildingFromAnimation();
    }
}
