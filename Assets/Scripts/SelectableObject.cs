using UnityEngine;


public class SelectableObject : MonoBehaviour
{
    public GameObject SelectionIndicator;
    public GameObject Badge;
    public HealthBar HealthBar;

    public virtual void Awake()
    {
        Unselect();
        if (Badge)
            Badge.SetActive(false);
        if (HealthBar)
            HealthBar.Hide();
    }

    public virtual void Start()
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
