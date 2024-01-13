using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SlideWithGameSpeed : MonoBehaviour
{
    public GameplayManager Manager;
    
    public bool flipDirection;
    public float speedModifier;

    public bool randomizeSpeedModifer;
    
    [Range(0,1)]
    public float randomRangeMin;
    [Min(1)]
    public float randomRangeMax;
    

    // Start is called before the first frame update
    void Start()
    {
        if (randomizeSpeedModifer)
        {
            speedModifier = Random.Range(randomRangeMin, randomRangeMax);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.down * (Manager.GameplaySpeed * Time.deltaTime * speedModifier * -flipDirection.ToSign()));
    }
}
