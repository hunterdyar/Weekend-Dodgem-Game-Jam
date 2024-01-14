using System;
using System.Collections;
using System.Collections.Generic;
using Blooper.TransitionEffects;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class CameraShake : MonoBehaviour
{
	private static CameraShake _instance;
	private static Coroutine _shakeRoutine;
	private static Camera _camera;

	public float defaultForce;
	public float defaultTime;
	private Vector3 _defaultCameraPosition;
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(this);
		}

		_camera = GetComponent<Camera>();
		_defaultCameraPosition = transform.localPosition;
	}

	public static void ShakeOnce(Vector3 dir, float force = 0, float time = 0)
	{
		if (force <= 0)
		{
			force = _instance.defaultForce;
		}

		if (time <= 0)
		{
			time = _instance.defaultTime;
		}
		if (_shakeRoutine != null)
		{
			_instance.StopCoroutine(_shakeRoutine);
			//if it's interupted, the camera won't be centered. This is a hack to fix walking.
			_camera.transform.position = _instance._defaultCameraPosition;
		}
		_shakeRoutine = _instance.StartCoroutine(ShakeOnceRoutine(dir, force, time));
	}

	private static IEnumerator ShakeOnceRoutine(Vector3 dir, float force, float time)
	{
		Vector3 start = _camera.transform.localPosition;
		Vector3 end = _camera.transform.localPosition + (dir.normalized * force);
		float t = 0;
		while (t < 1)
		{
			Vector3 elasticPos = Vector3.Lerp(start, end, OutElastic(t));
			_camera.transform.localPosition = Vector3.Lerp(start, elasticPos, InCirc(1 - t));
			t += Time.deltaTime / time;
			yield return null;
		}

		_camera.transform.position = start;
	}

	//https: //easings.net/#easeOutElastic
	//implemented from
	//https://gist.github.com/Kryzarel/bba64622057f21a1d6d44879f9cd7bd4
	public static float InElastic(float t) => 1 - OutElastic(1 - t);

	public static float OutElastic(float t)
	{
		float p = 0.3f;
		return (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - p / 4) * (2 * Math.PI) / p) + 1;
	}

	public static float InOutElastic(float t)
	{
		if (t < 0.5) return InElastic(t * 2) / 2;
		return 1 - InElastic((1 - t) * 2) / 2;
	}

	public static float InCirc(float t) => -((float)Math.Sqrt(1 - t * t) - 1);
	public static float OutCirc(float t) => 1 - InCirc(1 - t);

	public static float InOutCirc(float t)
	{
		if (t < 0.5) return InCirc(t * 2) / 2;
		return 1 - InCirc((1 - t) * 2) / 2;
	}
	
}
