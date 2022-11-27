using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWorker : MonoBehaviour
{
    private Worker _targetUnit;
    private bool FindClosestUnit()
    {
        _targetUnit = UnitsManager.Instance.GetClousestWorker(transform.position);
        return _targetUnit != null;
    }
}
