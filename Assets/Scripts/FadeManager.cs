
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blooper.TransitionEffects;
using DefaultNamespace;
using Unity.VisualScripting;

public class FadeManager : MonoBehaviour
{
	public Transform fadeTarget;
	public Coroutine fadeRoutine;
	public GameplayManager Manager;
	public float fadeTime = 0.6f;
	void Start()
	{
		FadeIn();
	}

	private void OnEnable()
	{
		GameplayManager.OnGameStateChange += OnGameStateChange;
	}

	private void OnDisable()
	{
		GameplayManager.OnGameStateChange -= OnGameStateChange;
	}

	private void OnGameStateChange(GameState state)
	{
		if (state == GameState.GameOver)
		{
			if (fadeRoutine != null)
			{
				StopCoroutine(fadeRoutine);
				fadeRoutine = null;
			}
			fadeRoutine = StartCoroutine(FadeOut());
		}
	}

    public void FadeIn()
    {
	    var rf = TransitionEffectUtility.FindTransitionEffectRenderFeature();
	    var cam = Camera.main;
	    
	    if (fadeRoutine != null)
	    {
		    StopCoroutine(fadeRoutine);
		    fadeRoutine = null;
	    }
	    
	    rf.SetCenter(TransitionEffectUtility.WorldSpaceToUV(cam, fadeTarget.position));
	    fadeRoutine = StartCoroutine(Transition.TransitionInToScene(TransitionType.CircleWipe, 0.05f, fadeTime, Color.black));
    }

    public IEnumerator FadeOut()
    {
	    yield return new WaitForSeconds(0.25f);
	    //don't need to do this every time.
	    var rf = TransitionEffectUtility.FindTransitionEffectRenderFeature();
	    var cam = Camera.main;
	    
	    rf.SetCenter(TransitionEffectUtility.WorldSpaceToUV(cam, fadeTarget.position));
	    yield return StartCoroutine(Transition.TransitionOutToColor(TransitionType.CircleWipe, 0, fadeTime, Color.black));
	    Manager.FadeEnded();
    }
}
