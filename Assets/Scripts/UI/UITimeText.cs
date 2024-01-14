using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class UITimeText : MonoBehaviour
{
    public TMP_Text _currentTimeText;
    public TMP_Text _bestTimeText;
    
    [SerializeField] private GameplayManager Manager;
    private static TimeSpan _span;
    private void Awake()
    {
        _span = new TimeSpan(0);
    }

    private void Start()
    {
        SetText(_bestTimeText, Manager.GetHighscore());
    }

    void Update()
    {
        _currentTimeText.enabled = false;
        
        _currentTimeText.enabled = true;
        float time = Manager.SurvivalTime;
        SetText(_currentTimeText,time);
        if (Manager.IsHighscore)
        {
            SetText(_bestTimeText,time);
        }

        if (Manager.GameState == GameState.GameOver)
        {
            //SetText(_bestTimeText, time);
        }
    }

    public static void SetText(TMP_Text text, float seconds)
    {
        _span = TimeSpan.FromSeconds(seconds);
        text.text = $"{_span.Minutes}:{_span.Seconds}:{_span.Milliseconds}";
    }
}
