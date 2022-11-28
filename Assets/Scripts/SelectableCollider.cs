using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
	public SelectableObject SelectableObject;
	public bool IntersectingWithSth = false;

	private void OnTriggerEnter(Collider other)
	{
		IntersectingWithSth = true;
	}

	private void OnTriggerStay(Collider other)
	{
		IntersectingWithSth = true;
	}

	private void OnTriggerExit(Collider other)
	{
		IntersectingWithSth = false;
	}
}
