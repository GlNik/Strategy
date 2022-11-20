using UnityEngine;

public class EnemyBuilding : SelectableObject
{

	[SerializeField] private int _health = 200;
	private int _maxHealth;

	private void Awake()
	{
		_maxHealth = _health;
	}
	private void Start()
	{
        WinManager.Instance.AddEnemyBuilding(this);
    }

	public override void Select()
	{
		base.Select();
	}
	public override void Unselect()
	{
		base.Unselect();
	}

	public void DestroyBuilding()
	{
        WinManager.Instance.RemoveEnemyBuilding(this);
        Destroy(gameObject);
    }

	public void TakeDamage(int damageValue)
	{
		_health -= damageValue;

		if (_health <= 0)
			DestroyBuilding();

		HealthBar.SetHealth(_health, _maxHealth);
	}
}
