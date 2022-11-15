using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Building
{
    [SerializeField] Transform _spawn;
    public void CreateUnit(GameObject unitPrefab)
    {
        GameObject newUnit = Instantiate(unitPrefab, _spawn.position, Quaternion.identity);
        Vector3 position = _spawn.position + new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1));
        newUnit.GetComponent<Unit>().WhenClickOnGround(position);
    }
}
