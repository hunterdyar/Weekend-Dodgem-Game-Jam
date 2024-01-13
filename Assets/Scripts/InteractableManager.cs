using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableManager : MonoBehaviour
{
    public GameObject[] InteractablePrefabs;
    public GameplayManager Manager;
    [SerializeField] private BoxCollider2D _spawnBox;
    private float _timer;
    
    void Update()
    {
        if (Manager.GameState == GameState.Gameplay)
        {
            _timer = _timer - Time.deltaTime - (Manager.GameplaySpeed * 0.2f * Time.deltaTime);
            if (_timer < 0)
            {
                ResetTimer();
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
        Vector3 position = GetSpawnPoint();
        var prefab = InteractablePrefabs[Random.Range(0, InteractablePrefabs.Length)];
        Instantiate(prefab, position, prefab.transform.rotation);
    }

    private Vector3 GetSpawnPoint()
    {
        return _spawnBox.RandomPointInBox();
    }

    void ResetTimer()
    {
        _timer = 1;
    }
}
