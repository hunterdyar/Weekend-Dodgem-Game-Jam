using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class UITimeText : MonoBehaviour
{
    private TMP_Text _text;

    [SerializeField] private GameplayManager Manager;
    private TimeSpan _span;
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _span = new TimeSpan(0);
    }

    // Update is called once per frame
    void Update()
    {
        _text.enabled = false;
        if (Manager.GameState == GameState.GameOver)
        {
            _text.enabled = true;
            float time = Manager.SurvivalTime;
            _span = TimeSpan.FromSeconds(time);
            _text.text = $"{_span.Minutes}:{_span.Seconds}:{_span.Milliseconds}";
        }
        
    }
}
