using UnityEngine;

public class CreateMainBuilding : MonoBehaviour
{
    [SerializeField] Building _mainBuilding;
    void Start()
    {
        Instantiate(_mainBuilding, transform.position, Quaternion.identity);        
    }
}
