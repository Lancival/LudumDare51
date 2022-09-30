using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    void Start()
    {
        Instantiate(playerPrefab);
    }
}
