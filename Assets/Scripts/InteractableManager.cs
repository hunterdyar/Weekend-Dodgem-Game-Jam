using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableManager : MonoBehaviour
{
    public GameplayManager Manager;
    [SerializeField] private BoxCollider2D _spawnBox;
    private float _timer;

    public WeightedSelection<GameObject> InteractablePrefabs;
    
    public float totalGameTimeForWeightsCurve;
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

            if (Manager.SurvivalTime < totalGameTimeForWeightsCurve && totalGameTimeForWeightsCurve != 0)
            {
                InteractablePrefabs.SetAllItemWeightsByCurve(Manager.SurvivalTime / totalGameTimeForWeightsCurve);
            }
        }
    }

    private void SpawnItem()
    {
        Vector3 position = GetSpawnPoint();
        var prefab = InteractablePrefabs.GetWeightedRandomItem();
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
