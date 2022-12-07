using UnityEngine;

public class GetSpawnPoint : MonoBehaviour
{

    public static GetSpawnPoint Instance { get; private set; }
    public Transform SpawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SpawnPoint = GetComponent<Transform>();
    }

    public Vector3 GetSpawnPointTransform()
    {
        return SpawnPoint.position;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
