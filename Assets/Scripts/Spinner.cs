using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class Spinner : MonoBehaviour
	{
		public float minSpinSpeed;
		public float maxSpinSpeed;

		private float _spinSpeed;

		private void Awake()
		{
			_spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
		}

		private void Update()
		{
			transform.Rotate(Vector3.back, _spinSpeed * Time.deltaTime);
		}
	}
}