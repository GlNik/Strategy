using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform ScaleTransform;

    public void SetHealth(int health, int maxHealth)
    {
        float xScale = Mathf.Clamp01((float)health / maxHealth);
        ScaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
