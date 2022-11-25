using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : Building
{
    private void Start()
    {
       
        WinManager.Instance.AddOurBuilding(this);
    }

    private void Update()
    {
        if (BuildingIsPlaced)
        {
           
        }

    }

    public void StartCoroutinePlusMoney()
    {
       
    }

    private void OnDestroy()
    {
        WinManager.Instance.RemoveOutBuilding(this);
    }
}
