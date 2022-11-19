using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SelectionIndicator;
    public GameObject Badge;
    public HealthBar HealthBar;

    private void Awake()
    {
        Unselect();
        if (Badge)
            Badge.SetActive(false);
        if (HealthBar)
            HealthBar.Hide();
    }

    private void Start()
    {
        SelectionIndicator.SetActive(false);
    }

    public virtual void OnHover()
    {
        if (Badge)
            Badge.SetActive(true);
    }
    public virtual void OnUnhover()
    {
        if (Badge)
            Badge.SetActive(false);
    }

    public virtual void Select()
    {
        SelectionIndicator.SetActive(true);
        if (HealthBar)
            HealthBar.Show();
    }

    public virtual void Unselect()
    {
        if (SelectionIndicator)
            SelectionIndicator.SetActive(false);

        if (HealthBar)
            HealthBar.Hide();
    }

    public virtual void WhenClickOnGround(Vector3 point)
    {

    }


}
