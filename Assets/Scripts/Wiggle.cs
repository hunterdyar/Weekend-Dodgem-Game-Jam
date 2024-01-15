using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class Wiggle : MonoBehaviour
	{
		public Vector2 offsetMin;
		public Vector2 offsetMax;
		public float rotationMin;
		public float rotationMax;
		public float speedMin;
		public float speedMax;
		[SerializeField] private float speed;
		private float offsetPhase;
		private float rotationPhase;
		private float ot;
		private float rt;

		private Vector3 initialLocal;
		private float initialRot;
		private void Awake()
		{
			initialRot = transform.rotation.eulerAngles.z;
			initialLocal = transform.localPosition;
			speed = Random.Range(speedMin, speedMax);
			offsetPhase = Random.value * 2 * Mathf.PI;
			rotationPhase = Random.value * 2 * Mathf.PI;
		}

		private void Update()
		{
			ot = Mathf.Sin((Time.time + offsetPhase)* speed / 2 /Mathf.PI)/2+.5f;
			rt = Mathf.Sin((Time.time + rotationPhase) * speed / 2/Mathf.PI)/2+.5f;
			transform.localPosition = initialLocal + Vector3.Lerp(offsetMin, offsetMax, ot);
			transform.rotation = Quaternion.Euler(0,0,initialRot+Mathf.Lerp(rotationMin,rotationMax,rt));
		}
	}
}